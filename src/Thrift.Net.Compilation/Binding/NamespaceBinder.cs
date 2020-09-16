namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="NamespaceStatementContext" /> objects into
    /// <see cref="Namespace" /> objects.
    /// </summary>
    public class NamespaceBinder : Binder<NamespaceStatementContext, Namespace>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceBinder" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        public NamespaceBinder(IBinder parent)
            : base(parent)
        {
        }

        /// <inheritdoc />
        protected override Namespace Bind(NamespaceStatementContext node)
        {
            var builder = new NamespaceBuilder()
                .SetNode(node)
                .SetScope(node.namespaceScope?.Text)
                .SetNamespaceName(node.ns?.Text);

            return builder.Build();
        }
    }
}