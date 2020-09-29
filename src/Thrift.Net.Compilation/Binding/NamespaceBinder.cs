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
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceBinder" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        /// <param name="binderProvider">Used to get binders for nodes.</param>
        public NamespaceBinder(IBinder parent, IBinderProvider binderProvider)
            : base(parent)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override Namespace Bind(NamespaceStatementContext node, ISymbol parent)
        {
            var builder = new NamespaceBuilder()
                .SetNode(node)
                .SetParent(parent as Document)
                .SetBinderProvider(this.binderProvider)
                .SetScope(node.namespaceScope?.Text)
                .SetNamespaceName(node.ns?.Text);

            return builder.Build();
        }
    }
}