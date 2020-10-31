namespace Thrift.Net.Compilation.Symbols
{
    using System;
    using System.Collections.Generic;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Binding;

    /// <summary>
    /// A base class for lists or sets.
    /// </summary>
    /// <typeparam name="TNode">The type of node the symbol is bound to.</typeparam>
    public abstract class ListOrSetType<TNode> : CollectionType<TNode>, IListOrSetType
        where TNode : IParseTree
    {
        private readonly IBinderProvider binderProvider;
        private readonly string csharpCollectionTypeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOrSetType{TNode}" /> class.
        /// </summary>
        /// <param name="node">The node this symbol is bound to.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <param name="binderProvider">Used to get binders for nodes.</param>
        /// <param name="csharpCollectionTypeName">The name of the C# type that will be generated from this type.</param>
        protected ListOrSetType(
            TNode node,
            ISymbol parent,
            IBinderProvider binderProvider,
            string csharpCollectionTypeName)
            : base(node, parent)
        {
            this.binderProvider = binderProvider;
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
        protected override IReadOnlyCollection<ISymbol> Children =>
            this.ElementType != null ? new List<ISymbol> { this.ElementType } : new List<ISymbol>();

        /// <summary>
        /// Gets the Antlr tree node representing the collection's element type.
        /// </summary>
        /// <returns>The element node.</returns>
        protected abstract IParseTree GetElementNode();

        /// <inheritdoc/>
        protected override string GetTypeName()
        {
            if (this.ElementType != null)
            {
                return $"{this.csharpCollectionTypeName}<{this.ElementType.CSharpRequiredTypeName}>";
            }

            throw new InvalidOperationException("Cannot get the type name because no element type was provided.");
        }
    }
}