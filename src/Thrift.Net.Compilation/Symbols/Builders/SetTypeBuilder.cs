namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Builds <see cref="SetType" /> objects.
    /// </summary>
    public class SetTypeBuilder : SymbolBuilder<SetTypeContext, SetType, ISymbol, SetTypeBuilder>
    {
        /// <inheritdoc/>
        public override SetType Build()
        {
            return new SetType(this.Node, this.Parent, this.BinderProvider);
        }
    }
}