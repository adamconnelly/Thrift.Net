namespace Thrift.Net.Compilation.Types
{
    using System;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// A base class for symbols that represent collections, like list or set.
    /// </summary>
    public abstract class CollectionType : ICollectionType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionType" /> class.
        /// </summary>
        protected CollectionType()
        {
        }

        /// <inheritdoc/>
        public INamedTypeSymbol Definition => throw new NotImplementedException("A collection type cannot have a definition");

        /// <inheritdoc/>
        public int? NestingDepth
        {
            get
            {
                // TODO: We'll need to figure out how to implement this without knowing about the parent
                return 0;

                // if (this.ContainingType is ICollectionType)
                // {
                //     var depth = 1;
                //     var parent = this.ContainingType;
                //     while (parent.ContainingType is ICollectionType)
                //     {
                //         parent = parent.ContainingType;
                //         depth++;
                //     }
                //
                //     return depth;
                // }

                // return null;
            }
        }

        /// <inheritdoc/>
        public string Name => this.GetThriftTypeName();

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

        /// <inheritdoc/>
        public bool IsAssignableFrom(IType expressionType)
        {
            return false;
        }

        /// <summary>
        /// Gets the C# type name for the collection.
        /// </summary>
        /// <returns>
        /// The type name.
        /// </returns>
        protected abstract string GetTypeName();

        /// <summary>
        /// Gets the Thrift type name for the collection.
        /// </summary>
        /// <returns>
        /// The thrift type name.
        /// </returns>
        protected abstract string GetThriftTypeName();
    }
}