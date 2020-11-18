namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift union.
    /// </summary>
    public interface IUnion : ISymbol<UnionDefinitionContext, IDocument>, INamedTypeSymbol, IFieldContainer
    {
    }
}