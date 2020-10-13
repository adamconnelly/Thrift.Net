namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Builds <see cref="ListType" /> objects.
    /// </summary>
    public class ListTypeBuilder : SymbolBuilder<ListTypeContext, ListType, ISymbol, ListTypeBuilder>
    {
        /// <inheritdoc/>
        public override ListType Build()
        {
            return new ListType(
                this.Node,
                this.Parent,
                this.BinderProvider);
        }
    }
}