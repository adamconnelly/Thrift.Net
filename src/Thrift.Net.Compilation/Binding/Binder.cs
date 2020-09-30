namespace Thrift.Net.Compilation.Binding
{
    using System;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// A base class for binders.
    /// </summary>
    /// <typeparam name="TNode">The type of node this binder can bind.</typeparam>
    /// <typeparam name="TResult">The type of symbol produced by the binder.</typeparam>
    public abstract class Binder<TNode, TResult> : IBinder
        where TNode : class, IParseTree
        where TResult : ISymbol
    {
        /// <inheritdoc />
        public TSymbol Bind<TSymbol>(IParseTree node, ISymbol parent)
            where TSymbol : class, ISymbol
        {
            if (!(node is TNode))
            {
                throw new InvalidOperationException($"This binder can only bind {typeof(TNode).Name} nodes");
            }

            var boundSymbol = this.Bind(node as TNode, parent);
            if (!(boundSymbol is TSymbol result))
            {
                throw new InvalidOperationException($"This binder can only be used to bind {typeof(TResult).Name} objects");
            }

            return result;
        }

        /// <summary>
        /// Binds the specified node, returning its symbol.
        /// </summary>
        /// <param name="node">The node to bind.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <returns>The bound symbol.</returns>
        protected abstract TResult Bind(TNode node, ISymbol parent);
    }
}