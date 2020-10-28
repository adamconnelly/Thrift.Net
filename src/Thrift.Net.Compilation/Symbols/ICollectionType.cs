namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Represents a collection type, like a list or set.
    /// </summary>
    public interface ICollectionType : IFieldType
    {
        /// <summary>
        /// Gets the level of nesting of this set. A top level set returns null,
        /// and each inner type returns an increasing number starting at 1 to
        /// indicate how deeply nested the set is.
        /// </summary>
        int? NestingDepth { get; }
    }
}