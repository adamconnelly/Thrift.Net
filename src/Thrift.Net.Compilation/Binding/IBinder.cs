namespace Thrift.Net.Compilation.Binding
{
    using System;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// An object that can bind a node in the parse tree to its semantic
    /// representation.
    /// </summary>
    public interface IBinder
    {
        /// <summary>
        /// Creates a symbol based on the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to get the symbol for.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <typeparam name="TSymbol">The type of the symbol.</typeparam>
        /// <returns>The symbol.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the symbol created from the node cannot be converted to
        /// <typeparamref name="TSymbol" />.
        /// </exception>
        TSymbol Bind<TSymbol>(IParseTree node, ISymbol parent)
            where TSymbol : class, ISymbol;
    }
}