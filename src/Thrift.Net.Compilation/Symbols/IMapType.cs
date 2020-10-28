namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift map.
    /// </summary>
    public interface IMapType : ISymbol<MapTypeContext, ISymbol>, ICollectionType
    {
        /// <summary>
        /// Gets the type of the map's keys.
        /// </summary>
        IFieldType KeyType { get; }

        /// <summary>
        /// Gets the type of the map's values.
        /// </summary>
        IFieldType ValueType { get; }
    }
}