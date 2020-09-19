namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// Represents a symbol that has a name, like a struct or enum.
    /// </summary>
    /// <typeparam name="TNode">
    /// The type of the node associated with the symbol.
    /// </typeparam>
    public abstract class NamedSymbol<TNode> : Symbol<TNode>, INamedSymbol
        where TNode : IParseTree
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedSymbol{TNode}" /> class.
        /// </summary>
        /// <param name="node">The node that this symbol was bound from.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <param name="name">The name of the symbol.</param>
        protected NamedSymbol(TNode node, ISymbol parent, string name)
            : base(node, parent)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the symbol.
        /// </summary>
        public string Name { get; }
    }
}