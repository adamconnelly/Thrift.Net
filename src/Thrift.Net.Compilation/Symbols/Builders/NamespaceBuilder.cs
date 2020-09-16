namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Namespace" /> objects.
    /// </summary>
    public class NamespaceBuilder
    {
        /// <summary>
        /// Gets the node used to create the namespace object.
        /// </summary>
        public NamespaceStatementContext Node { get; private set; }

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
        /// Sets the node used to create the namespace object.
        /// </summary>
        /// <param name="node">The node used to create the namespace object.</param>
        /// <returns>The builder.</returns>
        public NamespaceBuilder SetNode(NamespaceStatementContext node)
        {
            this.Node = node;

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

        /// <summary>
        /// Builds the namespace.
        /// </summary>
        /// <returns>The namespace.</returns>
        public Namespace Build()
        {
            return new Namespace(this.Node, this.Scope, this.NamespaceName);
        }
    }
}