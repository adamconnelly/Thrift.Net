namespace Thrift.Net.Compilation.Types
{
    /// <summary>
    /// Represents a Thrift map.
    /// </summary>
    public interface IMapType : ICollectionType
    {
        /// <summary>
        /// Gets the type of the map's keys.
        /// </summary>
        IType KeyType { get; }

        /// <summary>
        /// Gets the type of the map's values.
        /// </summary>
        IType ValueType { get; }
    }
}