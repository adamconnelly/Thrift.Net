namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Antlr;

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
        /// Finds the symbol that represents the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to search for.</param>
        /// <returns>The symbol matching the node, or null if no symbol was found.</returns>
        Document FindSymbolForNode(ThriftParser.DocumentContext node);

        /// <summary>
        /// Finds the symbol that represents the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to search for.</param>
        /// <returns>The symbol matching the node, or null if no symbol was found.</returns>
        Enum FindSymbolForNode(ThriftParser.EnumDefinitionContext node);

        /// <summary>
        /// Finds the symbol that represents the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to search for.</param>
        /// <returns>The symbol matching the node, or null if no symbol was found.</returns>
        EnumMember FindSymbolForNode(ThriftParser.EnumMemberContext node);

        /// <summary>
        /// Finds the symbol that represents the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to search for.</param>
        /// <returns>The symbol matching the node, or null if no symbol was found.</returns>
        Field FindSymbolForNode(ThriftParser.FieldContext node);

        /// <summary>
        /// Finds the symbol that represents the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to search for.</param>
        /// <returns>The symbol matching the node, or null if no symbol was found.</returns>
        Namespace FindSymbolForNode(ThriftParser.NamespaceStatementContext node);

        /// <summary>
        /// Finds the symbol that represents the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to search for.</param>
        /// <returns>The symbol matching the node, or null if no symbol was found.</returns>
        Struct FindSymbolForNode(ThriftParser.StructDefinitionContext node);

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="typeName">The name of the type to resolve.</param>
        /// <returns>The type, or null if the type could not be resolved.</returns>
        FieldType ResolveType(string typeName);

        /// <summary>
        /// Accepts the visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Accept(ISymbolVisitor visitor);
    }

    /// <summary>
    /// Represents a symbol in the semantic model.
    /// </summary>
    /// <typeparam name="TNode">
    /// The type of the node associated with the symbol.
    /// </typeparam>
    /// <typeparam name="TParent">The type of the symbol's parent.</typeparam>
    public interface ISymbol<TNode, TParent> : ISymbol
        where TNode : IParseTree
        where TParent : ISymbol
    {
        /// <summary>
        /// Gets the node this symbol was bound from.
        /// </summary>
        new TNode Node { get; }

        /// <summary>
        /// Gets the parent symbol.
        /// </summary>
        new TParent Parent { get; }
    }
}