namespace Thrift.Net.Compilation
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Thrift.Net.Antlr;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// A visitor used to perform the main compilation.
    /// </summary>
    public class CompilationVisitor : ThriftBaseVisitor<int?>
    {
        private readonly List<CompilationMessage> messages = new List<CompilationMessage>();
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationVisitor" /> class.
        /// </summary>
        /// <param name="binderProvider">Used to get binders for tree nodes.</param>
        public CompilationVisitor(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <summary>
        /// Gets the document.
        /// </summary>
        public Document Document { get; private set; }

        /// <summary>
        /// Gets any messages reported during analysis.
        /// </summary>
        public IReadOnlyCollection<CompilationMessage> Messages => this.messages;

        /// <inheritdoc />
        public override int? VisitDocument([NotNull] DocumentContext context)
        {
            var documentBinder = this.binderProvider.GetBinder(context);

            // TODO: Pass in container symbol
            this.Document = documentBinder.Bind<Document>(context, null);

            var result = base.VisitDocument(context);

            return result;
        }

        /// <inheritdoc />
        public override int? VisitNamespaceStatement(
            NamespaceStatementContext context)
        {
            var result = base.VisitNamespaceStatement(context);

            var @namespace = this.Document.FindSymbolForNode(context);

            if (@namespace.Scope != null && !@namespace.HasKnownScope)
            {
                // The namespace scope is not in the list of known scopes.
                // For example `namespace notalang mynamespace`
                this.AddError(
                    CompilerMessageId.NamespaceScopeUnknown,
                    context.namespaceScope,
                    context.namespaceScope.Text);
            }

            if (@namespace.Scope == null && @namespace.NamespaceName == null)
            {
                // Both the namespace and scope are missing. For example `namespace`.
                this.AddError(
                    CompilerMessageId.NamespaceAndScopeMissing,
                    context.NAMESPACE().Symbol);
            }
            else if (@namespace.NamespaceName == null)
            {
                // The namespace is missing. For example `namespace csharp`
                this.AddError(
                    CompilerMessageId.NamespaceMissing,
                    context.NAMESPACE().Symbol,
                    context.ns ?? context.namespaceScope ?? context.NAMESPACE().Symbol);
            }
            else if (@namespace.Scope == null)
            {
                // The namespace scope is missing. For example
                // `namespace mynamespace`
                this.AddError(
                    CompilerMessageId.NamespaceScopeMissing,
                    context.NAMESPACE().Symbol,
                    context.ns);
            }

            if (@namespace.Scope != null)
            {
                if (this.Document.IsNamespaceForScopeAlreadyDeclared(@namespace))
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

            if (@namespace.Node.separator != null)
            {
                // The namespace statement includes a list separator, which is
                // not allowed. For example:
                // ```
                // namespace csharp Thrift.Net.Examples,
                // namespace netstd Thrift.Net.Examples;
                // ```
                this.AddError(
                    CompilerMessageId.NamespaceStatementTerminatedBySeparator,
                    @namespace.Node.separator,
                    @namespace.Node.separator.Text);
            }

            return result;
        }

        /// <inheritdoc />
        public override int? VisitEnumDefinition(EnumDefinitionContext context)
        {
            var result = base.VisitEnumDefinition(context);

            var enumDefinition = this.Document.FindSymbolForNode(context);

            this.AddEnumMessages(enumDefinition);

            return result;
        }

        /// <inheritdoc />
        public override int? VisitEnumMember(ThriftParser.EnumMemberContext context)
        {
            var result = base.VisitEnumMember(context);

            var enumMember = this.Document.FindSymbolForNode(context);

            this.AddEnumMemberMessages(enumMember);

            return result;
        }

        /// <inheritdoc />
        public override int? VisitStructDefinition([NotNull] ThriftParser.StructDefinitionContext context)
        {
            var result = base.VisitStructDefinition(context);

            var structDefinition = this.Document.FindSymbolForNode(context);

            if (structDefinition.Name == null)
            {
                // A struct has been declared with no name. For example `struct {}`.
                this.AddError(
                    CompilerMessageId.StructMustHaveAName,
                    context.STRUCT().Symbol);
            }
            else
            {
                if (this.Document.IsMemberNameAlreadyDeclared(structDefinition))
                {
                    // Another type has already been declared with the same name.
                    // For example:
                    // ```
                    // enum User {}
                    // struct User {}
                    // ```
                    this.AddError(
                        CompilerMessageId.NameAlreadyDeclared,
                        context.name,
                        structDefinition.Name);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override int? VisitField([NotNull] FieldContext context)
        {
            var field = this.Document.FindSymbolForNode(context);

            if (((Struct)field.Parent).IsFieldNameAlreadyDefined(field.Name, context))
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
                    context.name,
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
                    context.name,
                    field.Name);
            }
            else
            {
                if (field.FieldId != null)
                {
                    if (((Struct)field.Parent).IsFieldIdAlreadyDefined(field.FieldId.Value, context))
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
                            context.fieldId,
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
                        context.fieldId,
                        field.RawFieldId);
                }
            }

            if (field.Type == FieldType.SList)
            {
                // A field has been declared using the slist type. For example:
                // ```
                // struct User {
                //     1: slist Username
                // }
                // ```
                this.AddWarning(
                    CompilerMessageId.SlistDeprecated,
                    context.fieldType().IDENTIFIER().Symbol,
                    field.Name);
            }

            if (!field.Type.IsResolved)
            {
                // A field has referenced a type that doesn't exist. For example:
                // ```
                // struct User {
                //     1: UserType Type
                // }
                // ```
                this.AddError(
                    CompilerMessageId.UnknownType,
                    field.Node.fieldType().IDENTIFIER().Symbol,
                    field.Type.Name);
            }

            return base.VisitField(context);
        }

        private void AddEnumMessages(Enum enumDefinition)
        {
            if (enumDefinition.Name == null)
            {
                // The enum name is missing: `enum {}`.
                this.AddError(
                    CompilerMessageId.EnumMustHaveAName,
                    enumDefinition.Node.ENUM().Symbol);
            }

            if (enumDefinition.Name != null &&
                ((Document)enumDefinition.Parent).IsMemberNameAlreadyDeclared(
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

        private void AddEnumMemberMessages(EnumMember enumMember)
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

            if (((Enum)enumMember.Parent).IsEnumMemberAlreadyDeclared(enumMember.Name, enumMember.Node))
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