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
    }
}