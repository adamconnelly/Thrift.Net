namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// Represents a symbol in the semantic model.
    /// </summary>
    public interface ISymbol
    {
    }

    /// <summary>
    /// Represents a symbol in the semantic model.
    /// </summary>
    /// <typeparam name="TNode">
    /// The type of the node associated with the symbol.
    /// </typeparam>
    public interface ISymbol<TNode> : ISymbol
        where TNode : IParseTree
    {
        /// <summary>
        /// Gets the node this symbol was created from.
        /// </summary>
        TNode Node { get; }
    }
}