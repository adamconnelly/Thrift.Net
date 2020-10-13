namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Represents a Thrift list.
    /// </summary>
    public interface IListType : IFieldType
    {
        /// <summary>
        /// Gets the type of element this list contains.
        /// </summary>
        IFieldType ElementType { get; }
    }
}