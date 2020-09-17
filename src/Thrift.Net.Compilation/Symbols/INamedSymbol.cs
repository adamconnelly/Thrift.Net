namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Represents a symbol that has a name, for example a struct or an enum.
    /// </summary>
    public interface INamedSymbol : ISymbol
    {
        /// <summary>
        /// Gets the name of the symbol.
        /// </summary>
        string Name { get; }
    }
}