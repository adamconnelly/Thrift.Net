namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Binds <see cref="IConstant" /> objects from <see cref="ConstDefinitionContext" /> nodes.
    /// </summary>
    public class ConstantBinder : Binder<ConstDefinitionContext, IConstant, IDocument>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">Used to get binders.</param>
        public ConstantBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        protected override IConstant Bind(ConstDefinitionContext node, IDocument parent)
        {
            var builder = new ConstantBuilder()
                .SetBinderProvider(this.binderProvider)
                .SetNode(node)
                .SetParent(parent)
                .SetName(node.name?.Text);

            return builder.Build();
        }
    }
}