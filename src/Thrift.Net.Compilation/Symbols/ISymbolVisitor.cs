namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Visits Symbols.
    /// </summary>
    public interface ISymbolVisitor
    {
        /// <summary>
        /// Visits a document.
        /// </summary>
        /// <param name="document">The document to visit.</param>
        void VisitDocument(IDocument document);

        /// <summary>
        /// Visits an enum.
        /// </summary>
        /// <param name="enum">The enum to visit.</param>
        void VisitEnum(IEnum @enum);

        /// <summary>
        /// Visits an enum member.
        /// </summary>
        /// <param name="enumMember">The enum member to visit.</param>
        void VisitEnumMember(IEnumMember enumMember);

        /// <summary>
        /// Visits a field.
        /// </summary>
        /// <param name="field">The field to visit.</param>
        void VisitField(IField field);

        /// <summary>
        /// Visits a namespace.
        /// </summary>
        /// <param name="namespace">The namespace to visit.</param>
        void VisitNamespace(INamespace @namespace);

        /// <summary>
        /// Visits a struct.
        /// </summary>
        /// <param name="struct">The struct to visit.</param>
        void VisitStruct(IStruct @struct);

        /// <summary>
        /// Visits a list type reference.
        /// </summary>
        /// <param name="listType">The list type being referenced by a field.</param>
        void VisitListType(IListType listType);

        /// <summary>
        /// Visits a reference to a user-defined type.
        /// </summary>
        /// <param name="userType">The user type being referenced by a field.</param>
        void VisitUserType(IUserType userType);

        /// <summary>
        /// Visits a constant.
        /// </summary>
        /// <param name="constant">The constant to visit.</param>
        void VisitConstant(IConstant constant);

        /// <summary>
        /// Visits a set type reference.
        /// </summary>
        /// <param name="setType">The set type being referenced by a field.</param>
        void VisitSetType(ISetType setType);

        /// <summary>
        /// Visits a map type reference.
        /// </summary>
        /// <param name="mapType">The map type being reference by a field.</param>
        void VisitMapType(MapType mapType);

        /// <summary>
        /// Visits a union.
        /// </summary>
        /// <param name="union">The union to visit.</param>
        void VisitUnion(IUnion union);

        /// <summary>
        /// Visits an exception.
        /// </summary>
        /// <param name="exception">The exception to visit.</param>
        void VisitException(IException exception);
    }
}