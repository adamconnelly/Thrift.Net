namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Binds <see cref="MapType" /> objects from <see cref="MapTypeContext" />.
    /// </summary>
    public class MapTypeBinder : Binder<MapTypeContext, MapType, ISymbol>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapTypeBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">Used to get binders.</param>
        public MapTypeBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        protected override MapType Bind(MapTypeContext node, ISymbol parent)
        {
            return new MapTypeBuilder()
                .SetNode(node)
                .SetParent(parent)
                .SetBinderProvider(this.binderProvider)
                .Build();
        }
    }
}