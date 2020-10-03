namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift struct.
    /// </summary>
    public interface IStruct : ISymbol<StructDefinitionContext, IDocument>, INamedSymbol
    {
        /// <summary>
        /// Gets the fields of the struct.
        /// </summary>
        IReadOnlyCollection<Field> Fields { get; }

        /// <summary>
        /// Gets the fields that are optional (either explicitly or implicitly).
        /// </summary>
        IReadOnlyCollection<Field> OptionalFields { get; }

        /// <summary>
        /// Checks whether a field with the specified Id has already been defined.
        /// </summary>
        /// <param name="fieldId">The field Id to check for.</param>
        /// <param name="node">The field being defined.</param>
        /// <returns>
        /// true if the field Id has already been defined, false otherwise.
        /// </returns>
        bool IsFieldIdAlreadyDefined(int fieldId, FieldContext node);

        /// <summary>
        /// Checks whether the field has already been defined.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="node">The field being defined.</param>
        /// <returns>
        /// true if the field name has already been defined, false otherwise.
        /// </returns>
        bool IsFieldNameAlreadyDefined(string name, FieldContext node);
    }
}