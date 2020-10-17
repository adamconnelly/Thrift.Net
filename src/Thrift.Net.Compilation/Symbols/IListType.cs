namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift list.
    /// </summary>
    public interface IListType : ISymbol<ListTypeContext, ISymbol>, IFieldType
    {
        /// <summary>
        /// Gets the type of element this list contains.
        /// </summary>
        IFieldType ElementType { get; }
    }
}