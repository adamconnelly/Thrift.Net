namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Builds a <see cref="MapType" /> object.
    /// </summary>
    public class MapTypeBuilder : SymbolBuilder<MapTypeContext, MapType, ISymbol, MapTypeBuilder>
    {
        /// <inheritdoc/>
        public override MapType Build()
        {
            return new MapType(this.Node, this.Parent, this.BinderProvider);
        }
    }
}