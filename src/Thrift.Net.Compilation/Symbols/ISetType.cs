namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift set.
    /// </summary>
    public interface ISetType : ISymbol<SetTypeContext, ISymbol>, ICollectionType
    {
        /// <summary>
        /// Gets the type of element this set contains.
        /// </summary>
        IFieldType ElementType { get; }
    }
}