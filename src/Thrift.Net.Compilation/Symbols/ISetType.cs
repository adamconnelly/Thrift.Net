namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift set.
    /// </summary>
    public interface ISetType : ISymbol<SetTypeContext, ISymbol>, ICollectionType
    {
    }
}