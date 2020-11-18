namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind a <see cref="Struct" /> from the parse tree.
    /// </summary>
    public class UnionBinder : Binder<UnionDefinitionContext, Union, IDocument>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnionBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">
        /// Used to get the correct binder for a particular node.
        /// </param>
        public UnionBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override Union Bind(UnionDefinitionContext node, IDocument parent)
        {
            var builder = new UnionBuilder()
                .SetNode(node)
                .SetParent(parent)
                .SetBinderProvider(this.binderProvider)
                .SetName(node.name?.Text);

            return builder.Build();
        }
    }
}