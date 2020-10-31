namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Represents a list or set.
    /// </summary>
    public interface IListOrSetType : ICollectionType
    {
        /// <summary>
        /// Gets the type of the collection's element.
        /// </summary>
        IFieldType ElementType { get; }
    }
}