namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Represents a named type, like an enum or struct.
    /// </summary>
    public interface INamedTypeSymbol : INamedSymbol
    {
        /// <summary>
        /// Gets the document this type is defined in.
        /// </summary>
        IDocument Document { get; }
    }
}