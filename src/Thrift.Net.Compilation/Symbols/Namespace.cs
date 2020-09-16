namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a namespace statement.
    /// </summary>
    public class Namespace : Symbol<NamespaceStatementContext>
    {
        /// <summary>
        /// The scope that applies to all language targets.
        /// </summary>
        public const string AllNamespacesScope = "*";

        // The list of scopes that the Thrift.Net compiler can use for generation.
        private static readonly HashSet<string> CSharpScopes =
            new HashSet<string>
            {
                "csharp", "netcore", "netstd",
            };

        // The set of scopes known the the Thrift.Net compiler. This can be used
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
        /// <param name="scope">
        /// The generator (i.e. language) that this namespace applies to.
        /// </param>
        /// <param name="namespaceName">The namespace.</param>
        public Namespace(
            NamespaceStatementContext node, string scope, string namespaceName)
            : base(node)
        {
            this.Scope = scope;
            this.NamespaceName = namespaceName;
        }

        /// <summary>
        /// Gets the scope of the namespace.
        /// </summary>
        public string Scope { get; }

        /// <summary>
        /// Gets a value indicating whether the compiler can use this namespace
        /// when generating the C# code. NOTE: this doesn't mean that this namespace
        /// will definitely be used if there are multiple namespaces declared with
        /// scopes that the compiler can handle.
        /// </summary>
        public bool CanGenerate
        {
            get
            {
                return this.Scope == AllNamespacesScope ||
                    IsCSharpNamespaceScope(this.Scope);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scope is known by the compiler.
        /// </summary>
        public bool HasKnownScope
        {
            get
            {
                return KnownNamespaceScopes.Contains(this.Scope);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this namespace applies to all code
        /// generation targets (i.e. has the `*` scope).
        /// </summary>
        public bool AppliesToAllTargets
        {
            get
            {
                return this.Scope == AllNamespacesScope;
            }
        }

        /// <summary>
        /// Gets the namespace name.
        /// </summary>
        public string NamespaceName { get; }

        /// <summary>
        /// Checks whether the scope is a csharp scope.
        /// </summary>
        /// <param name="scope">The scope to check.</param>
        /// <returns>true if the scope is a C# scope. false otherwise.</returns>
        public static bool IsCSharpNamespaceScope(string scope)
        {
            return CSharpScopes.Contains(scope);
        }
    }
}