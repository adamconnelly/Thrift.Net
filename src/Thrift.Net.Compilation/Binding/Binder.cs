namespace Thrift.Net.Compilation.Binding
{
    using System;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Symbols;

    // Allow multiple types in this file since we're just overloading a generic class.
    #pragma warning disable SA1402

    /// <summary>
    /// A base class for binders.
    /// </summary>
    /// <typeparam name="TNode">The type of node this binder can bind.</typeparam>
    /// <typeparam name="TResult">The type of symbol produced by the binder.</typeparam>
    public abstract class Binder<TNode, TResult> : Binder<TNode, TResult, IBinder>
        where TNode : class, IParseTree
        where TResult : ISymbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Binder{TNode, TResult}" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        protected Binder(IBinder parent)
            : base(parent)
        {
        }
    }

    /// <summary>
    /// A base class for binders.
    /// </summary>
    /// <typeparam name="TNode">The type of node this binder can bind.</typeparam>
    /// <typeparam name="TResult">The type of symbol produced by the binder.</typeparam>
    /// <typeparam name="TParentBinder">The type of the parent binder.</typeparam>
    public abstract class Binder<TNode, TResult, TParentBinder> : IBinder
        where TNode : class, IParseTree
        where TResult : ISymbol
        where TParentBinder : IBinder
    {
        private readonly TParentBinder parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Binder{TNode, TResult, TParentBinder}" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        protected Binder(TParentBinder parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Gets the parent binder.
        /// </summary>
        protected TParentBinder Parent => this.parent;

        /// <inheritdoc />
        public virtual FieldType ResolveType(string typeName)
        {
            return this.parent?.ResolveType(typeName);
        }

        /// <inheritdoc />
        public TSymbol Bind<TSymbol>(IParseTree node)
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