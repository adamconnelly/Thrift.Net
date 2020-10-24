namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Binds <see cref="SetType" /> Symbols from <see cref="SetTypeContext" /> nodes.
    /// </summary>
    public class SetTypeBinder : Binder<SetTypeContext, SetType, ISymbol>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetTypeBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">Used to get binders.</param>
        public SetTypeBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        protected override SetType Bind(SetTypeContext node, ISymbol parent)
        {
            return new SetTypeBuilder()
                .SetNode(node)
                .SetParent(parent)
                .SetBinderProvider(this.binderProvider)
                .Build();
        }
    }
}