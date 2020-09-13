namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// Represents a symbol in the semantic model.
    /// </summary>
    public interface ISymbol
    {
        /// <summary>
        /// Gets the node this symbol was created from.
        /// </summary>
        IParseTree Node { get; }
    }
}