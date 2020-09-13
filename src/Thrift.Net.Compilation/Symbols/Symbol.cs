namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// The base class for symbols.
    /// </summary>
    public abstract class Symbol : ISymbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol" /> class.
        /// </summary>
        /// <param name="node">The node associated with this symbol.</param>
        protected Symbol(IParseTree node)
        {
            this.Node = node;
        }

        /// <inheritdoc />
        public IParseTree Node { get; private set; }
    }
}