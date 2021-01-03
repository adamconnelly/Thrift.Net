namespace Thrift.Net.Compilation.Symbols
{
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
        public virtual void VisitUserType(IUserType userType)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitSetType(ISetType setType)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitMapType(MapType mapType)
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
    }
}