namespace Thrift.Net.Compilation
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Antlr;
    using Thrift.Net.Compilation.Model;

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

        private readonly List<EnumDefinition> enums = new List<EnumDefinition>();
        private readonly List<CompilationMessage> messages = new List<CompilationMessage>();
        private readonly ParseTreeProperty<EnumMember> enumMembers = new ParseTreeProperty<EnumMember>();

        // Used to store the current value of an enum so we can automatically generate
        // values if they aren't defined explicitly.
        private readonly ParseTreeProperty<int> currentEnumValue = new ParseTreeProperty<int>();

        /// <summary>
        /// Gets the namespace of the document.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// Gets the enums defined in the document.
        /// </summary>
        public IReadOnlyCollection<EnumDefinition> Enums => this.enums;

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
                this.messages.Add(new CompilationMessage(
                    CompilerMessageId.NamespaceAndScopeMissing,
                    CompilerMessageType.Error,
                    context.NAMESPACE().Symbol.Line,
                    context.NAMESPACE().Symbol.Column + 1,
                    context.NAMESPACE().Symbol.Column + context.NAMESPACE().Symbol.Text.Length));
            }
            else if (context.namespaceScope == null)
            {
                if (AllowedNamespaceScopes.Contains(context.ns.Text))
                {
                    // The namespace is missing. For example `namespace csharp`
                    this.messages.Add(new CompilationMessage(
                        CompilerMessageId.NamespaceMissing,
                        CompilerMessageType.Error,
                        context.NAMESPACE().Symbol.Line,
                        context.NAMESPACE().Symbol.Column + 1,
                        context.ns.Column + context.ns.Text.Length));
                }
                else
                {
                    // The namespace scope is missing. For example
                    // `namespace mynamespace`
                    this.messages.Add(new CompilationMessage(
                        CompilerMessageId.NamespaceScopeMissing,
                        CompilerMessageType.Error,
                        context.NAMESPACE().Symbol.Line,
                        context.NAMESPACE().Symbol.Column + 1,
                        context.ns.Column + context.ns.Text.Length));
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override int? VisitEnumDefinition(ThriftParser.EnumDefinitionContext context)
        {
            this.currentEnumValue.Put(context, 0);

            var result = base.VisitEnumDefinition(context);

            this.currentEnumValue.RemoveFrom(context);

            var name = this.GetEnumName(context);
            var members = context.enumMember().Select(member => this.enumMembers.Get(member));

            this.enums.Add(new EnumDefinition(name, members.ToList()));

            return result;
        }

        /// <inheritdoc />
        public override int? VisitEnumMember(ThriftParser.EnumMemberContext context)
        {
            var result = base.VisitEnumMember(context);

            var name = this.GetEnumMemberName(context);
            var value = this.GetEnumValue(context);

            var enumMember = new EnumMember(name, value);

            this.enumMembers.Put(context, enumMember);

            if (context.enumValue != null && context.EQUALS_OPERATOR() == null)
            {
                // An enum value has been specified without the = operator.
                // For example `enum UserType { User 10 }`
                this.messages.Add(new CompilationMessage(
                    CompilerMessageId.EnumMemberEqualsOperatorMissing,
                    CompilerMessageType.Error,
                    context.IDENTIFIER().Symbol.Line,
                    context.IDENTIFIER().Symbol.Column + 1,
                    context.enumValue.Column + context.enumValue.Text.Length));
            }

            return result;
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
                this.messages.Add(new CompilationMessage(
                    CompilerMessageId.NamespaceScopeUnknown,
                    CompilerMessageType.Error,
                    context.namespaceScope.Line,
                    context.namespaceScope.Column + 1,
                    context.namespaceScope.Column + context.namespaceScope.Text.Length));
            }
        }

        private string GetEnumName(ThriftParser.EnumDefinitionContext context)
        {
            if (context.IDENTIFIER() != null)
            {
                return context.IDENTIFIER().Symbol.Text;
            }

            // The enum name is missing: `enum {}`.
            this.messages.Add(new CompilationMessage(
                CompilerMessageId.EnumMustHaveAName,
                CompilerMessageType.Error,
                context.ENUM().Symbol.Line,
                context.ENUM().Symbol.Column + 1,
                context.ENUM().Symbol.Column + context.ENUM().Symbol.Text.Length));

            return null;
        }

        private string GetEnumMemberName(ThriftParser.EnumMemberContext context)
        {
            if (context.IDENTIFIER() != null)
            {
                return context.IDENTIFIER().Symbol.Text;
            }

            // The enum member name is missing: `= 1`
            this.messages.Add(new CompilationMessage(
                CompilerMessageId.EnumMemberMustHaveAName,
                CompilerMessageType.Error,
                context.EQUALS_OPERATOR().Symbol.Line,
                context.EQUALS_OPERATOR().Symbol.Column + 1,
                context.enumValue.Column + context.enumValue.Text.Length));

            return null;
        }

        private int GetEnumValue(ThriftParser.EnumMemberContext context)
        {
            // According to the Thrift IDL specification, if an enum value is
            // not supplied, it should either be:
            //   - 0 for the first element in an enum.
            //   - P+1 - where `P` is the value of the previous element.
            var currentValue = this.currentEnumValue.Get(context.Parent);
            if (context.enumValue != null)
            {
                if (int.TryParse(context.enumValue.Text, out var value))
                {
                    currentValue = value;

                    if (value < 0)
                    {
                        // A negative enum value has been specified: `User = -1`.
                        this.messages.Add(new CompilationMessage(
                            CompilerMessageId.EnumValueMustNotBeNegative,
                            CompilerMessageType.Error,
                            context.enumValue.Line,
                            context.enumValue.Column + 1,
                            context.enumValue.Column + context.enumValue.Text.Length));
                    }
                }
                else
                {
                    // A non-integer enum value has been specified: `User = "test"`.
                    this.messages.Add(new CompilationMessage(
                        CompilerMessageId.EnumValueMustBeAnInteger,
                        CompilerMessageType.Error,
                        context.enumValue.Line,
                        context.enumValue.Column + 1,
                        context.enumValue.Column + context.enumValue.Text.Length));
                }
            }
            else if (context.EQUALS_OPERATOR() != null)
            {
                // An enum member has been defined with an equals sign, but a
                // missing value: `User = `.
                this.messages.Add(new CompilationMessage(
                        CompilerMessageId.EnumValueMustBeSpecified,
                        CompilerMessageType.Error,
                        context.IDENTIFIER().Symbol.Line,
                        context.IDENTIFIER().Symbol.Column + 1,
                        context.EQUALS_OPERATOR().Symbol.Column + context.EQUALS_OPERATOR().Symbol.Text.Length));
            }

            this.currentEnumValue.Put(context.Parent, currentValue + 1);

            return currentValue;
        }
    }
}