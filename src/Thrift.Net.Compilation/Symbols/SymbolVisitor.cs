namespace Thrift.Net.Compilation.Symbols
{
    using Thrift.Net.Compilation.Types;

    /// <summary>
    /// A base class for Symbol visitors.
    /// </summary>
    public abstract class SymbolVisitor : ISymbolVisitor
    {
        /// <inheritdoc/>
        public virtual void VisitDocument(IDocument document)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitEnum(IEnum @enum)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitEnumMember(IEnumMember enumMember)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitField(IField field)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitNamespace(INamespace @namespace)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitStruct(IStruct @struct)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitListType(IListType listType)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitSetType(ISetType setType)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitMapType(IMapType mapType)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitUnion(IUnion union)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitException(IException exception)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitConstant(IConstant constant)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitFieldType(IFieldType fieldType)
        {
        }
    }
}