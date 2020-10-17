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
    }
}