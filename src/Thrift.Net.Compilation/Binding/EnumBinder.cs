namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="EnumDefinitionContext" /> objects to
    /// <see cref="Enum" /> objects.
    /// </summary>
    public class EnumBinder : Binder<EnumDefinitionContext, Enum>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">The binder provider.</param>
        public EnumBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override Enum Bind(EnumDefinitionContext node, ISymbol parent)
        {
            // TODO: Can we make the parent typed?
            var builder = new EnumBuilder()
                .SetNode(node)
                .SetParent(parent as Document)
                .SetBinderProvider(this.binderProvider)
                .SetName(node.name?.Text);

            return builder.Build();
        }
    }
}