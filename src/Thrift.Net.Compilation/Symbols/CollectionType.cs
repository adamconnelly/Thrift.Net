namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// A base class for symbols that represent collections, like list or set.
    /// </summary>
    /// <typeparam name="TNode">The type of node this symbol is bound to.</typeparam>
    public abstract class CollectionType<TNode> : Symbol<TNode, ISymbol>, ICollectionType
        where TNode : IParseTree
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionType{TNode}" /> class.
        /// </summary>
        /// <param name="node">The node this symbol is bound to.</param>
        /// <param name="parent">The parent symbol.</param>
        protected CollectionType(
            TNode node,
            ISymbol parent)
            : base(node, parent)
        {
        }

        /// <inheritdoc/>
        public int? NestingDepth
        {
            get
            {
                if (this.Parent is ICollectionType)
                {
                    var depth = 1;
                    var parent = this.Parent;
                    while (parent.Parent is ICollectionType)
                    {
                        parent = parent.Parent;
                        depth++;
                    }

                    return depth;
                }

                return null;
            }
        }

        /// <inheritdoc/>
        public string Name => this.Node.GetText();

        /// <inheritdoc/>
        public bool IsResolved => true;

        /// <inheritdoc/>
        public string CSharpOptionalTypeName => this.GetTypeName();

        /// <inheritdoc/>
        public string CSharpRequiredTypeName => this.GetTypeName();

        /// <inheritdoc/>
        public bool IsBaseType => false;

        /// <inheritdoc/>
        public bool IsStruct => false;

        /// <inheritdoc/>
        public bool IsEnum => false;

        /// <inheritdoc/>
        public bool IsList => this is IListType;

        /// <inheritdoc/>
        public bool IsCollection => true;

        /// <inheritdoc/>
        public bool IsSet => this is ISetType;

        /// <inheritdoc/>
        public bool IsMap => this is IMapType;

        /// <summary>
        /// Gets the C# type name for the collection.
        /// </summary>
        /// <returns>
        /// The type name.
        /// </returns>
        protected abstract string GetTypeName();
    }
}