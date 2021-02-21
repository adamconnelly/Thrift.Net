namespace Thrift.Net.Compilation.Types
{
    using System;

    /// <summary>
    /// A base class for lists or sets.
    /// </summary>
    public abstract class ListOrSetType : CollectionType, IListOrSetType
    {
        private readonly string csharpCollectionTypeName;
        private readonly string thriftCollectionTypeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOrSetType" /> class.
        /// </summary>
        /// <param name="csharpCollectionTypeName">The name of the C# type that will be generated from this type.</param>
        /// <param name="thriftCollectionTypeName">The name of the Thrift collection that this type represents.</param>
        /// <param name="elementType">The collection's element type.</param>
        protected ListOrSetType(
            string csharpCollectionTypeName,
            string thriftCollectionTypeName,
            IType elementType)
        {
            this.csharpCollectionTypeName = csharpCollectionTypeName ?? throw new ArgumentNullException(nameof(csharpCollectionTypeName));
            this.thriftCollectionTypeName = thriftCollectionTypeName ?? throw new ArgumentNullException(nameof(thriftCollectionTypeName));
            this.ElementType = elementType;
        }

        /// <inheritdoc/>
        public IType ElementType { get; }

        /// <inheritdoc/>
        protected override string GetTypeName()
        {
            return $"{this.csharpCollectionTypeName}<{this.ElementType?.CSharpRequiredTypeName}>";
        }

        /// <inheritdoc/>
        protected override string GetThriftTypeName()
        {
            return $"{this.thriftCollectionTypeName}<{this.ElementType?.Name}>";
        }
    }
}