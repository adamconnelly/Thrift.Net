namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a namespace statement.
    /// </summary>
    public class Namespace : Symbol<NamespaceStatementContext, IDocument>, INamespace
    {
        /// <summary>
        /// The scope that applies to all language targets.
        /// </summary>
        public const string AllNamespacesScope = "*";

        // The set of scopes that the Thrift.Net compiler can use for generation.
        private static readonly HashSet<string> CSharpScopes =
            new HashSet<string>
            {
                "csharp", "netcore", "netstd",
            };

        // The set of scopes known to the Thrift.Net compiler. This can be used
        // to warn about invalid scopes.
        private static readonly HashSet<string> KnownNamespaceScopes =
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
                "netstd",
                "perl",
                "php",
                "py.twisted",
                "py",
                "rb",
                "st",
                "xsd",
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="Namespace" /> class.
        /// </summary>
        /// <param name="node">The node the namespace was created from.</param>
        /// <param name="parent">The document containing this namespace.</param>
        /// <param name="scope">
        /// The generator (i.e. language) that this namespace applies to.
        /// </param>
        /// <param name="namespaceName">The namespace.</param>
        public Namespace(
            NamespaceStatementContext node,
            IDocument parent,
            string scope,
            string namespaceName)
            : base(node, parent)
        {
            this.Scope = scope;
            this.NamespaceName = namespaceName;
        }

        /// <inheritdoc/>
        public string Scope { get; }

        /// <inheritdoc/>
        public bool HasCSharpScope
        {
            get
            {
                return CSharpScopes.Contains(this.Scope);
            }
        }

        /// <inheritdoc/>
        public bool HasKnownScope
        {
            get
            {
                return KnownNamespaceScopes.Contains(this.Scope);
            }
        }

        /// <inheritdoc/>
        public bool AppliesToAllTargets
        {
            get
            {
                return this.Scope == AllNamespacesScope;
            }
        }

        /// <inheritdoc/>
        public string NamespaceName { get; }

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitNamespace(this);
            base.Accept(visitor);
        }
    }
}