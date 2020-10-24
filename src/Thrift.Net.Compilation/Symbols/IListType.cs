namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift list.
    /// </summary>
    public interface IListType : ISymbol<ListTypeContext, ISymbol>, ICollectionType
    {
    }
}