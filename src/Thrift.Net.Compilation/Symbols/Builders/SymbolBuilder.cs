namespace Thrift.Net.Compilation.Symbols.Builders
{
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Binding;

    /// <summary>
    /// A base class for symbol builders.
    /// </summary>
    /// <typeparam name="TNode">The type of node associated with the symbol.</typeparam>
    /// <typeparam name="TSymbol">The type of symbol the builder builds.</typeparam>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    public abstract class SymbolBuilder<TNode, TSymbol, TBuilder>
        where TNode : IParseTree
        where TSymbol : ISymbol
        where TBuilder : SymbolBuilder<TNode, TSymbol, TBuilder>
    {
        /// <summary>
        /// Gets the node associated with the symbol.
        /// </summary>
        public TNode Node { get; private set; }

        /// <summary>
        /// Gets the binder provider.
        /// </summary>
        public IBinderProvider BinderProvider { get; private set; }

        /// <summary>
        /// Sets the node associated with the symbol.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The builder.</returns>
        public TBuilder SetNode(TNode node)
        {
            this.Node = node;

            return (TBuilder)this;
        }

        /// <summary>
        /// Sets the binder provider.
        /// </summary>
        /// <param name="binderProvider">The binder provider.</param>
        /// <returns>The builder.</returns>
        public TBuilder SetBinderProvider(IBinderProvider binderProvider)
        {
            this.BinderProvider = binderProvider;

            return (TBuilder)this;
        }

        /// <summary>
        /// Builds the symbol.
        /// </summary>
        /// <returns>The symbol.</returns>
        public abstract TSymbol Build();
    }
}