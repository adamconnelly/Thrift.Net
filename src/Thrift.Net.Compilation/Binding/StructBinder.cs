namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind a <see cref="Struct" /> from the parse tree.
    /// </summary>
    public class StructBinder : Binder<StructDefinitionContext, Struct>, IFieldContainerBinder
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">
        /// Used to get the correct binder for a particular node.
        /// </param>
        public StructBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        public FieldRequiredness DefaultFieldRequiredness => FieldRequiredness.Default;

        /// <inheritdoc />
        protected override Struct Bind(StructDefinitionContext node, ISymbol parent)
        {
            var builder = new StructBuilder()
                .SetNode(node)
                .SetParent(parent as Document)
                .SetBinderProvider(this.binderProvider)
                .SetName(node.name?.Text);

            return builder.Build();
        }
    }
}