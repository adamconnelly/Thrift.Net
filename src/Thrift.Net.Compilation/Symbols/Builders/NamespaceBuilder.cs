namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Namespace" /> objects.
    /// </summary>
    public class NamespaceBuilder : SymbolBuilder<NamespaceStatementContext, Namespace, IDocument, NamespaceBuilder>
    {
        /// <summary>
        /// Gets the code generation scope that this namespace applies to.
        /// </summary>
        public string Scope { get; private set; }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public string NamespaceName { get; private set; }

        /// <summary>
        /// Sets the scope.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <returns>The builder.</returns>
        public NamespaceBuilder SetScope(string scope)
        {
            this.Scope = scope;

            return this;
        }

        /// <summary>
        /// Sets the namespace.
        /// </summary>
        /// <param name="namespaceName">The namespace.</param>
        /// <returns>The builder.</returns>
        public NamespaceBuilder SetNamespaceName(string namespaceName)
        {
            this.NamespaceName = namespaceName;

            return this;
        }

        /// <inheritdoc/>
        public override Namespace Build()
        {
            return new Namespace(
                this.Node,
                this.Parent,
                this.Scope,
                this.NamespaceName);
        }
    }
}