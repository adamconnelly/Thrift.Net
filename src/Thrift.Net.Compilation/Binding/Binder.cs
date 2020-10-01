namespace Thrift.Net.Compilation.Binding
{
    using System;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// A base class for binders.
    /// </summary>
    /// <typeparam name="TNode">The type of node this binder can bind.</typeparam>
    /// <typeparam name="TSymbol">The type of symbol produced by the binder.</typeparam>
    /// <typeparam name="TParentSymbol">The type of the symbol's parent.</typeparam>
    public abstract class Binder<TNode, TSymbol, TParentSymbol> : IBinder
        where TNode : class, IParseTree
        where TSymbol : ISymbol
        where TParentSymbol : ISymbol
    {
        /// <inheritdoc />
        public TResult Bind<TResult>(IParseTree node, ISymbol parent)
            where TResult : class, ISymbol
        {
            if (!(node is TNode))
            {
                throw new InvalidOperationException($"This binder can only bind {typeof(TNode).Name} nodes");
            }

            var boundSymbol = this.Bind(node as TNode, (TParentSymbol)parent);
            if (!(boundSymbol is TResult result))
            {
                throw new InvalidOperationException($"This binder can only be used to bind {typeof(TSymbol).Name} objects");
            }

            return result;
        }

        /// <summary>
        /// Binds the specified node, returning its symbol.
        /// </summary>
        /// <param name="node">The node to bind.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <returns>The bound symbol.</returns>
        protected abstract TSymbol Bind(TNode node, TParentSymbol parent);
    }
}