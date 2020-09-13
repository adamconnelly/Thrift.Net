namespace Thrift.Net.Compilation
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Antlr;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;

    /// <summary>
    /// A visitor used to perform the main compilation.
    /// </summary>
    public class CompilationVisitor : ThriftBaseVisitor<int?>
    {
        /// <summary>
        /// The list of namespace scopes that we'll use to set the C# namespace.
        /// </summary>
        private static readonly HashSet<string> RecognisedNamespaceScopes =
            new HashSet<string>
            {
                "*",
                "csharp",
                "netcore",
            };

        /// <summary>
        /// The list of namespace scopes that are allowed without causing an
        /// error message to be reported.
        /// </summary>
        private static readonly HashSet<string> AllowedNamespaceScopes =
            new HashSet<string>
            {
                "*",
                "c_glib",
                "cpp",
                "csharp",
                "delphi",
                "go",
                "java",
                "js",
                "lua",
                "netcore",
                "perl",
                "php",
                "py",
                "py.twisted",
                "rb",
                "st",
                "xsd",
            };

        private readonly Dictionary<string, Enum> enums = new Dictionary<string, Enum>();
        private readonly List<Struct> structs = new List<Struct>();
        private readonly List<CompilationMessage> messages = new List<CompilationMessage>();
        private readonly ParseTreeProperty<EnumMember> enumMembers = new ParseTreeProperty<EnumMember>();
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
        /// Gets the namespace of the document.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// Gets the enums defined in the document.
        /// </summary>
        public IReadOnlyCollection<Enum> Enums => this.enums.Values;

        /// <summary>
        /// Gets the structs defined in the document.
        /// </summary>
        public IReadOnlyCollection<Struct> Structs => this.structs;

        /// <summary>
        /// Gets any messages reported during analysis.
        /// </summary>
        public IReadOnlyCollection<CompilationMessage> Messages => this.messages;

        /// <inheritdoc />
        public override int? VisitNamespaceStatement(
            ThriftParser.NamespaceStatementContext context)
        {
            var result = base.VisitNamespaceStatement(context);

            if (context.namespaceScope != null && context.ns != null)
            {
                this.SetNamespace(context);
            }
            else if (context.namespaceScope == null && context.ns == null)
            {
                // Both the namespace and scope are missing. For example `namespace`.
                this.AddError(
                    CompilerMessageId.NamespaceAndScopeMissing,
                    context.NAMESPACE().Symbol);
            }
            else if (context.namespaceScope == null)
            {
                if (AllowedNamespaceScopes.Contains(context.ns.Text))
                {
                    // The namespace is missing. For example `namespace csharp`
                    this.AddError(
                        CompilerMessageId.NamespaceMissing,
                        context.NAMESPACE().Symbol,
                        context.ns);
                }
                else
                {
                    // The namespace scope is missing. For example
                    // `namespace mynamespace`
                    this.AddError(
                        CompilerMessageId.NamespaceScopeMissing,
                        context.NAMESPACE().Symbol,
                        context.ns);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override int? VisitEnumDefinition(ThriftParser.EnumDefinitionContext context)
        {
            var result = base.VisitEnumDefinition(context);

            var enumBuilder = new EnumBuilder();
            enumBuilder.SetName(this.GetEnumName(context));
            this.GetEnumMembers(context, enumBuilder);
            var enumDefinition = enumBuilder.Build();

            if (enumDefinition.Name != null && !this.enums.TryAdd(enumDefinition.Name, enumDefinition))
            {
                this.AddError(
                    CompilerMessageId.EnumDuplicated,
                    context.name,
                    context.name.Text);
            }

            if (!enumDefinition.Members.Any())
            {
                var warningTarget = context.name ?? context.ENUM().Symbol;

                // The enum has no members. For example
                // `enum MyEnum {}`
                this.AddWarning(CompilerMessageId.EnumEmpty, warningTarget);
            }

            return result;
        }

        /// <inheritdoc />
        public override int? VisitEnumMember(ThriftParser.EnumMemberContext context)
        {
            var result = base.VisitEnumMember(context);

            var memberBinder = this.binderProvider.GetBinder(context);
            var enumMember = memberBinder.Bind<EnumMember>(context);

            this.enumMembers.Put(context, enumMember);

            this.AddEnumMemberMessages(context, enumMember);

            return result;
        }

        /// <inheritdoc />
        public override int? VisitStructDefinition([NotNull] ThriftParser.StructDefinitionContext context)
        {
            var result = base.VisitStructDefinition(context);

            var binder = this.binderProvider.GetBinder(context);
            var structDefinition = binder.Bind<Struct>(context);

            if (structDefinition.Name == null)
            {
                this.AddError(
                    CompilerMessageId.StructMustHaveAName,
                    context.STRUCT().Symbol);
            }

            this.structs.Add(structDefinition);

            return result;
        }

        /// <inheritdoc />
        public override int? VisitField([NotNull] ThriftParser.FieldContext context)
        {
            var fieldBinder = this.binderProvider.GetBinder(context);
            var field = fieldBinder.Bind<Field>(context);
            var parentBinder = this.binderProvider.GetBinder(context.Parent) as IFieldContainerBinder;
            if (parentBinder.IsFieldNameAlreadyDefined(field.Name, context))
            {
                this.AddError(
                    CompilerMessageId.StructFieldNameAlreadyDefined,
                    context.name,
                    field.Name);
            }

            if (!field.IsFieldIdImplicit)
            {
                if (field.FieldId != null)
                {
                    if (parentBinder.IsFieldIdAlreadyDefined(field.FieldId.Value, context))
                    {
                        this.AddError(
                            CompilerMessageId.StructFieldIdAlreadyDefined,
                            context.fieldId,
                            field.FieldId.ToString());
                    }
                }
                else
                {
                    this.AddError(
                        CompilerMessageId.StructFieldIdMustBeAPositiveInteger,
                        context.fieldId,
                        field.RawFieldId);
                }
            }

            return base.VisitField(context);
        }

        private void GetEnumMembers(
            ThriftParser.EnumDefinitionContext context, EnumBuilder enumBuilder)
        {
            var members = new HashSet<string>();

            foreach (var memberNode in context.enumMember())
            {
                var member = this.enumMembers.Get(memberNode);
                if (member.Name != null)
                {
                    if (members.Add(member.Name))
                    {
                        enumBuilder.AddMember(member);
                    }
                    else
                    {
                        this.AddError(
                            CompilerMessageId.EnumMemberDuplicated,
                            memberNode.IDENTIFIER().Symbol,
                            member.Name);
                    }
                }
            }
        }

        private void SetNamespace(ThriftParser.NamespaceStatementContext context)
        {
            if (RecognisedNamespaceScopes.Contains(context.namespaceScope.Text))
            {
                this.Namespace = context.ns.Text;
            }

            if (!AllowedNamespaceScopes.Contains(context.namespaceScope.Text))
            {
                // The namespace scope is not in the list of known scopes.
                // For example `namespace notalang mynamespace`
                this.AddError(
                    CompilerMessageId.NamespaceScopeUnknown,
                    context.namespaceScope,
                    context.namespaceScope.Text);
            }
        }

        private string GetEnumName(ThriftParser.EnumDefinitionContext context)
        {
            if (context.name != null)
            {
                return context.name.Text;
            }

            // The enum name is missing: `enum {}`.
            this.AddError(
                CompilerMessageId.EnumMustHaveAName,
                context.ENUM().Symbol);

            return null;
        }

        private void AddEnumMemberMessages(ThriftParser.EnumMemberContext context, EnumMember enumMember)
        {
            if (enumMember.Name == null)
            {
                // The enum member name is missing: `= 1`
                this.AddError(
                    CompilerMessageId.EnumMemberMustHaveAName,
                    context.EQUALS_OPERATOR().Symbol,
                    context.enumValue);
            }

            switch (enumMember.InvalidValueReason)
            {
                case InvalidEnumValueReason.Negative:
                    // A negative enum value has been specified: `User = -1`.
                    this.AddError(
                        CompilerMessageId.EnumValueMustNotBeNegative,
                        context.enumValue);
                    break;

                case InvalidEnumValueReason.NotAnInteger:
                    // A non-integer enum value has been specified: `User = "test"`.
                    this.AddError(
                        CompilerMessageId.EnumValueMustBeAnInteger,
                        context.enumValue);
                    break;

                case InvalidEnumValueReason.Missing:
                    // An enum member has been defined with an equals sign, but a
                    // missing value: `User = `.
                    this.AddError(
                        CompilerMessageId.EnumValueMustBeSpecified,
                        context.IDENTIFIER().Symbol,
                        context.EQUALS_OPERATOR().Symbol);
                    break;
            }

            if (context.enumValue != null && context.EQUALS_OPERATOR() == null)
            {
                // An enum value has been specified without the = operator.
                // For example `enum UserType { User 10 }`
                this.AddError(
                    CompilerMessageId.EnumMemberEqualsOperatorMissing,
                    context.IDENTIFIER().Symbol,
                    context.enumValue);
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