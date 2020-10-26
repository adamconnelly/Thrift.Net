namespace Thrift.Net.Compilation.Symbols
{
    using System;
    using System.Collections.Generic;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Binding;

    /// <summary>
    /// A base class for symbols that represent collections, like list or set.
    /// </summary>
    /// <typeparam name="TNode">The type of node this symbol is bound to.</typeparam>
    public abstract class CollectionType<TNode> : Symbol<TNode, ISymbol>, ICollectionType
        where TNode : IParseTree
    {
        private readonly IBinderProvider binderProvider;
        private readonly string collectionTypeName;
        private readonly string csharpCollectionTypeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionType{TNode}" /> class.
        /// </summary>
        /// <param name="node">The node this symbol is bound to.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <param name="binderProvider">Used to get binders for nodes.</param>
        /// <param name="collectionTypeName">The name of the Thrift collection type (e.g. `list`).</param>
        /// <param name="csharpCollectionTypeName">The name of the C# type that will be generated from this type.</param>
        protected CollectionType(
            TNode node,
            ISymbol parent,
            IBinderProvider binderProvider,
            string collectionTypeName,
            string csharpCollectionTypeName)
            : base(node, parent)
        {
            this.binderProvider = binderProvider;
            this.collectionTypeName = collectionTypeName;
            this.csharpCollectionTypeName = csharpCollectionTypeName;
        }

        /// <inheritdoc/>
        public IFieldType ElementType
        {
            get
            {
                var elementNode = this.GetElementNode();
                if (elementNode != null)
                {
                    return this.binderProvider
                        .GetBinder(elementNode)
                        .Bind<IFieldType>(elementNode, this);
                }

                return null;
            }
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
        public string Name => $"{this.collectionTypeName}<{this.ElementType?.Name}>";

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
        public abstract bool IsList { get; }

        /// <inheritdoc/>
        public bool IsCollection => true;

        /// <inheritdoc/>
        protected override IReadOnlyCollection<ISymbol> Children =>
            this.ElementType != null ? new List<ISymbol> { this.ElementType } : new List<ISymbol>();

        /// <summary>
        /// Gets the Antlr tree node representing the collection's element type.
        /// </summary>
        /// <returns>The element node.</returns>
        protected abstract IParseTree GetElementNode();

        private string GetTypeName()
        {
            if (this.ElementType != null)
            {
                return $"{this.csharpCollectionTypeName}<{this.ElementType.CSharpRequiredTypeName}>";
            }

            throw new InvalidOperationException("Cannot get the type name because no element type was provided.");
        }
    }
}