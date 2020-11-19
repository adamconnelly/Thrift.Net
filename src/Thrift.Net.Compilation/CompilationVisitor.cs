namespace Thrift.Net.Compilation
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// A visitor used to perform the main compilation.
    /// </summary>
    public class CompilationVisitor : SymbolVisitor
    {
        private readonly List<CompilationMessage> messages = new List<CompilationMessage>();

        /// <summary>
        /// Gets any messages reported during analysis.
        /// </summary>
        public IReadOnlyCollection<CompilationMessage> Messages => this.messages;

        /// <inheritdoc/>
        public override void VisitDocument(IDocument document)
        {
            if (!document.ContainsDefinitions)
            {
                // The document is empty - that means it is either literally empty,
                // or it only includes namespace and include statements, but no types.
                this.AddWarning(CompilerMessageId.DocumentEmpty, document.Node.Start);
            }

            base.VisitDocument(document);
        }

        /// <inheritdoc />
        public override void VisitNamespace(INamespace @namespace)
        {
            if (@namespace.Scope != null && !@namespace.HasKnownScope)
            {
                // The namespace scope is not in the list of known scopes.
                // For example `namespace notalang mynamespace`
                this.AddError(
                    CompilerMessageId.NamespaceScopeUnknown,
                    @namespace.Node.namespaceScope,
                    @namespace.Node.namespaceScope.Text);
            }

            if (@namespace.Scope == null && @namespace.NamespaceName == null)
            {
                // Both the namespace and scope are missing. For example `namespace`.
                this.AddError(
                    CompilerMessageId.NamespaceAndScopeMissing,
                    @namespace.Node.NAMESPACE().Symbol);
            }
            else if (@namespace.NamespaceName == null)
            {
                // The namespace is missing. For example `namespace csharp`
                this.AddError(
                    CompilerMessageId.NamespaceMissing,
                    @namespace.Node.NAMESPACE().Symbol,
                    @namespace.Node.ns ?? @namespace.Node.namespaceScope ?? @namespace.Node.NAMESPACE().Symbol);
            }
            else if (@namespace.Scope == null)
            {
                // The namespace scope is missing. For example
                // `namespace mynamespace`
                this.AddError(
                    CompilerMessageId.NamespaceScopeMissing,
                    @namespace.Node.NAMESPACE().Symbol,
                    @namespace.Node.ns);
            }

            if (@namespace.Scope != null)
            {
                if (((IDocument)@namespace.Parent).IsNamespaceForScopeAlreadyDeclared(@namespace))
                {
                    // The namespace scope has already been specified. For example:
                    // ```
                    // namespace csharp Thrift.Net.Examples.A
                    // namespace csharp Thrift.Net.Examples.B
                    // ```
                    this.AddError(
                        CompilerMessageId.NamespaceScopeAlreadySpecified,
                        @namespace.Node.namespaceScope,
                        @namespace.Scope);
                }
            }

            if (@namespace.Node.listSeparator() != null)
            {
                // The namespace statement includes a list separator, which is
                // not allowed. For example:
                // ```
                // namespace csharp Thrift.Net.Examples,
                // namespace netstd Thrift.Net.Examples;
                // ```
                var separator = @namespace.Node.listSeparator().SEMICOLON() ??
                    @namespace.Node.listSeparator().COMMA();
                this.AddError(
                    CompilerMessageId.NamespaceStatementTerminatedBySeparator,
                    separator.Symbol,
                    separator.GetText());
            }

            base.VisitNamespace(@namespace);
        }

        /// <inheritdoc />
        public override void VisitEnum(IEnum @enum)
        {
            this.AddEnumMessages(@enum);

            base.VisitEnum(@enum);
        }

        /// <inheritdoc />
        public override void VisitEnumMember(IEnumMember enumMember)
        {
            this.AddEnumMemberMessages(enumMember);

            base.VisitEnumMember(enumMember);
        }

        /// <inheritdoc />
        public override void VisitStruct(IStruct @struct)
        {
            if (@struct.Name == null)
            {
                // A struct has been declared with no name. For example `struct {}`.
                this.AddError(
                    CompilerMessageId.StructMustHaveAName,
                    @struct.Node.STRUCT().Symbol);
            }
            else
            {
                if (@struct.Parent.IsMemberNameAlreadyDeclared(@struct))
                {
                    // Another type has already been declared with the same name.
                    // For example:
                    // ```
                    // enum User {}
                    // struct User {}
                    // ```
                    this.AddError(
                        CompilerMessageId.NameAlreadyDeclared,
                        @struct.Node.name,
                        @struct.Name);
                }
            }

            base.VisitStruct(@struct);
        }

        /// <inheritdoc />
        public override void VisitField(IField field)
        {
            if (field.Parent.IsFieldNameAlreadyDefined(field.Name, field.Node))
            {
                // The field name has already been declared. For example:
                // ```
                // struct User {
                //     1: string Username
                //     2: string Username
                // }
                // ```
                this.AddError(
                    CompilerMessageId.StructFieldNameAlreadyDefined,
                    field.Node.name,
                    field.Name);
            }

            if (field.IsFieldIdImplicit)
            {
                // A field has been declared with no field Id. For example:
                // ```
                // struct User {
                //     string Username
                // }
                // ```
                this.AddWarning(
                    CompilerMessageId.FieldIdNotSpecified,
                    field.Node.name,
                    field.Name);
            }
            else
            {
                if (field.FieldId != null)
                {
                    if (field.Parent.IsFieldIdAlreadyDefined(field.FieldId.Value, field.Node))
                    {
                        // The field Id has already been declared. For example:
                        // ```
                        // struct User {
                        //     1: i32 Id
                        //     1: string Username
                        // }
                        // ```
                        this.AddError(
                            CompilerMessageId.StructFieldIdAlreadyDefined,
                            field.Node.fieldId,
                            field.FieldId.ToString());
                    }
                }
                else
                {
                    // A negative field Id has been specified. For example:
                    // ```
                    // struct User {
                    //     -1: string Username
                    // }
                    // ```
                    this.AddError(
                        CompilerMessageId.StructFieldIdMustBeAPositiveInteger,
                        field.Node.fieldId,
                        field.RawFieldId);
                }
            }

            if (field.Type.Name == BaseType.Slist)
            {
                // A field has been declared using a deprecated type. For example:
                // ```
                // struct User {
                //     1: slist Username
                // }
                // ```
                this.AddWarning(
                    CompilerMessageId.SlistDeprecated,
                    field.Node.fieldType().baseType().typeName,
                    field.Name);
            }

            if (field.Requiredness == FieldRequiredness.Required && field.Parent is IUnion)
            {
                // A field in a union has been marked as required.
                // ```
                // union Request {
                //     1: required string Query
                // }
                // ```
                this.AddError(
                    CompilerMessageId.UnionCannotContainRequiredFields,
                    field.Node.fieldRequiredness().Start,
                    field.Name);
            }

            base.VisitField(field);
        }

        /// <inheritdoc/>
        public override void VisitListType(IListType listType)
        {
            if (listType.ElementType == null)
            {
                // A list has been declared with no element type specified. For example:
                // ```
                // struct User {
                //   1: list<> Emails
                // }
                // ```
                this.AddError(
                    CompilerMessageId.ListMustHaveElementTypeSpecified,
                    listType.Node.LIST().Symbol,
                    listType.Node.GT_OPERATOR()?.Symbol ?? listType.Node.LIST().Symbol);
            }

            base.VisitListType(listType);
        }

        /// <inheritdoc/>
        public override void VisitUserType(IUserType userType)
        {
            if (!userType.IsResolved)
            {
                // A field has referenced a type that doesn't exist. For example:
                // ```
                // struct User {
                //     1: UserType Type
                // }
                // ```
                this.AddError(
                    CompilerMessageId.UnknownType,
                    userType.Node.IDENTIFIER().Symbol,
                    userType.Name);
            }

            base.VisitUserType(userType);
        }

        /// <inheritdoc/>
        public override void VisitSetType(ISetType setType)
        {
            if (setType.ElementType == null)
            {
                // A set has been declared with no element type specified. For example:
                // ```
                // struct User {
                //   1: set<> Emails
                // }
                // ```
                this.AddError(
                    CompilerMessageId.SetMustHaveElementTypeSpecified,
                    setType.Node.SET().Symbol,
                    setType.Node.GT_OPERATOR()?.Symbol ?? setType.Node.SET().Symbol);
            }

            base.VisitSetType(setType);
        }

        /// <inheritdoc/>
        public override void VisitMapType(MapType mapType)
        {
            if (mapType.KeyType == null && mapType.ValueType == null)
            {
                // A map has been declared with no key or value types specified.
                // For example:
                // ```
                // struct User {
                //   1: map<> Emails
                // }
                // ```
                this.AddError(
                    CompilerMessageId.MapMustHaveKeyAndValueTypeSpecified,
                    mapType.Node.MAP().Symbol,
                    mapType.Node.GT_OPERATOR()?.Symbol ?? mapType.Node.MAP().Symbol);
            }
            else if (mapType.KeyType == null)
            {
                // A map has been declared with no key type specified. For example:
                // ```
                // struct User {
                //   1: map<, string> Emails
                // }
                // ```
                this.AddError(
                    CompilerMessageId.MapMustHaveKeyTypeSpecified,
                    mapType.Node.MAP().Symbol,
                    mapType.Node.GT_OPERATOR().Symbol);
            }
            else if (mapType.ValueType == null)
            {
                // A map has been declared with no value type specified. For example:
                // ```
                // struct User {
                //   1: map<EmailType, > Emails
                // }
                // ```
                this.AddError(
                    CompilerMessageId.MapMustHaveValueTypeSpecified,
                    mapType.Node.MAP().Symbol,
                    mapType.Node.GT_OPERATOR().Symbol);
            }

            base.VisitMapType(mapType);
        }

        /// <inheritdoc/>
        public override void VisitUnion(IUnion union)
        {
            if (union.Name == null)
            {
                // A union has been declared with no name. For example `union {}`.
                this.AddError(
                    CompilerMessageId.UnionMustHaveAName,
                    union.Node.UNION().Symbol);
            }
            else if (union.Parent.IsMemberNameAlreadyDeclared(union))
            {
                // Another type has already been declared with the same name.
                // For example:
                // ```
                // struct Request {}
                // union Request {}
                // ```
                this.AddError(
                    CompilerMessageId.NameAlreadyDeclared,
                    union.Node.name,
                    union.Name);
            }

            base.VisitUnion(union);
        }

        private void AddEnumMessages(IEnum enumDefinition)
        {
            if (enumDefinition.Name == null)
            {
                // The enum name is missing: `enum {}`.
                this.AddError(
                    CompilerMessageId.EnumMustHaveAName,
                    enumDefinition.Node.ENUM().Symbol);
            }

            if (enumDefinition.Name != null &&
                enumDefinition.Parent.IsMemberNameAlreadyDeclared(
                    enumDefinition))
            {
                // Another type has already been declared with the same name:
                // ```
                // struct UserType {}
                // enum UserType {}
                // ```
                this.AddError(
                    CompilerMessageId.NameAlreadyDeclared,
                    enumDefinition.Node.name,
                    enumDefinition.Node.name.Text);
            }

            if (!enumDefinition.Members.Any())
            {
                var warningTarget = enumDefinition.Node.name ?? enumDefinition.Node.ENUM().Symbol;

                // The enum has no members. For example
                // `enum MyEnum {}`
                this.AddWarning(CompilerMessageId.EnumEmpty, warningTarget);
            }
        }

        private void AddEnumMemberMessages(IEnumMember enumMember)
        {
            if (enumMember.Name == null)
            {
                // The enum member name is missing: `= 1`
                this.AddError(
                    CompilerMessageId.EnumMemberMustHaveAName,
                    enumMember.Node.EQUALS_OPERATOR().Symbol,
                    enumMember.Node.enumValue);
            }

            switch (enumMember.InvalidValueReason)
            {
                case InvalidEnumValueReason.Negative:
                    // A negative enum value has been specified: `User = -1`.
                    this.AddError(
                        CompilerMessageId.EnumValueMustNotBeNegative,
                        enumMember.Node.enumValue);
                    break;

                case InvalidEnumValueReason.NotAnInteger:
                    // A non-integer enum value has been specified: `User = "test"`.
                    this.AddError(
                        CompilerMessageId.EnumValueMustBeAnInteger,
                        enumMember.Node.enumValue);
                    break;

                case InvalidEnumValueReason.InvalidHexValue:
                    // An invalid hex value has been specified: `User = 0xZZZ`.
                    this.AddError(
                        CompilerMessageId.EnumValueInvalidHex,
                        enumMember.Node.enumValue,
                        enumMember.RawValue);
                    break;

                case InvalidEnumValueReason.Missing:
                    // An enum member has been defined with an equals sign, but a
                    // missing value: `User = `.
                    this.AddError(
                        CompilerMessageId.EnumValueMustBeSpecified,
                        enumMember.Node.IDENTIFIER().Symbol,
                        enumMember.Node.EQUALS_OPERATOR().Symbol);
                    break;
            }

            if (enumMember.Node.enumValue != null && enumMember.Node.EQUALS_OPERATOR() == null)
            {
                // An enum value has been specified without the = operator.
                // For example `enum UserType { User 10 }`
                this.AddError(
                    CompilerMessageId.EnumMemberEqualsOperatorMissing,
                    enumMember.Node.IDENTIFIER().Symbol,
                    enumMember.Node.enumValue);
            }

            if (enumMember.Parent.IsEnumMemberAlreadyDeclared(enumMember))
            {
                // The same enum member has been declared twice:
                // ```
                // enum UserType {
                //     User
                //     User
                // }
                // ```
                this.AddError(
                    CompilerMessageId.EnumMemberDuplicated,
                    enumMember.Node.IDENTIFIER().Symbol,
                    enumMember.Name);
            }

            if (enumMember.Parent.IsEnumValueAlreadyDeclared(enumMember))
            {
                // The same enum value has already been used by another member:
                // ```
                // enum UserType {
                //     User = 1
                //     Administrator = 1
                // }
                // ```
                this.AddError(
                    CompilerMessageId.EnumValueDuplicated,
                    enumMember.Node.enumValue,
                    enumMember.RawValue);
            }

            if (enumMember.IsValueImplicit)
            {
                // The enum member doesn't have a value provided:
                // ```
                // enum UserType {
                //   User
                // }
                // ```
                this.AddWarning(
                    CompilerMessageId.EnumMemberHasImplicitValue,
                    enumMember.Node.IDENTIFIER().Symbol,
                    enumMember.Name,
                    enumMember.Value.ToString());
            }
        }

        private void AddError(CompilerMessageId messageId, IToken token, params string[] messageParameters)
        {
            this.messages.Add(CompilationMessage.CreateError(messageId, token, token, messageParameters));
        }

        private void AddError(CompilerMessageId messageId, IToken startToken, IToken endToken, params string[] messageParameters)
        {
            this.messages.Add(CompilationMessage.CreateError(
                messageId, startToken, endToken, messageParameters));
        }

        private void AddWarning(CompilerMessageId messageId, IToken token, params string[] messageParameters)
        {
            this.messages.Add(CompilationMessage.CreateWarning(
                messageId, token, token, messageParameters));
        }
    }
}