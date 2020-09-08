namespace Thrift.Net.Compilation.Binding
{
    using System;
    using Antlr4.Runtime;
    using Thrift.Net.Compilation.Model;

    /// <summary>
    /// A base class for binders.
    /// </summary>
    /// <typeparam name="TNode">The type of node this binder can bind.</typeparam>
    /// <typeparam name="TResult">The type of symbol produced by the binder.</typeparam>
    public abstract class Binder<TNode, TResult> : IBinder
        where TNode : ParserRuleContext
        where TResult : ISymbol
    {
        private readonly IBinder parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Binder{TNode, TResult}" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        protected Binder(IBinder parent)
        {
            this.parent = parent;
        }

        /// <inheritdoc />
        public virtual FieldType ResolveType(string typeName)
        {
            return this.parent.ResolveType(typeName);
        }

        /// <inheritdoc />
        public TSymbol Bind<TSymbol>(ParserRuleContext node)
            where TSymbol : class, ISymbol
        {
            if (!(node is TNode))
            {
                throw new InvalidOperationException($"This binder can only bind {typeof(TNode).Name} nodes");
            }

            var boundSymbol = this.Bind(node as TNode);
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
        /// <returns>The bound symbol.</returns>
        protected abstract TResult Bind(TNode node);
    }
}