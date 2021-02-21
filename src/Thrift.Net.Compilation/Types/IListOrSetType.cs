namespace Thrift.Net.Compilation.Types
{
    /// <summary>
    /// Represents a list or set.
    /// </summary>
    public interface IListOrSetType : ICollectionType
    {
        /// <summary>
        /// Gets the type of the collection's element.
        /// </summary>
        IType ElementType { get; }
    }
}