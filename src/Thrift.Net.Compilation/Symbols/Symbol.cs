namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// The base class for symbols.
    /// </summary>
    /// <typeparam name="TNode">
    /// The type of the node associated with the symbol.
    /// </typeparam>
    public abstract class Symbol<TNode> : ISymbol<TNode>
        where TNode : IParseTree
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol{T}" /> class.
        /// </summary>
        /// <param name="node">The node associated with this symbol.</param>
        /// <param name="parent">The parent symbol.</param>
        protected Symbol(TNode node, ISymbol parent)
        {
            this.Node = node;
            this.Parent = parent;
        }

        /// <inheritdoc />
        IParseTree ISymbol.Node => this.Node;

        /// <inheritdoc />
        public TNode Node { get; private set; }

        /// <inheritdoc />
        public ISymbol Parent { get; }
    }
}