namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// Represents a symbol in the semantic model.
    /// </summary>
    public interface ISymbol
    {
        /// <summary>
        /// Gets the node this symbol was bound from.
        /// </summary>
        IParseTree Node { get; }

        /// <summary>
        /// Gets the parent symbol. This can be used to walk up the tree for
        /// type resolution.
        /// </summary>
        ISymbol Parent { get; }

        /// <summary>
        /// Finds the symbol that represents the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to search for.</param>
        /// <returns>The symbol matching the node, or null if no symbol was found.</returns>
        ISymbol FindSymbolForNode(IParseTree node);

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="typeName">The name of the type to resolve.</param>
        /// <returns>The type, or null if the type could not be resolved.</returns>
        FieldType ResolveType(string typeName);
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
        /// Gets the node this symbol was bound from.
        /// </summary>
        new TNode Node { get; }
    }
}