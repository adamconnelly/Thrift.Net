namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a field in a struct, union or exception.
    /// </summary>
    public interface IField : ISymbol<FieldContext>
    {
        /// <summary>
        /// Gets the field's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the Id of the field. This will be null if the field Id could
        /// not be parsed from the IDL, for example if it is negative or not
        /// an integer. The <see cref="RawFieldId" /> field can be used to get
        /// the raw string value.
        /// </summary>
        int? FieldId { get; }

        /// <summary>
        /// Gets the raw text of the field Id rather than the integer value
        /// stored in <see cref="FieldId" />.
        /// </summary>
        string RawFieldId { get; }

        /// <summary>
        /// Gets the level of requiredness of this field.
        /// </summary>
        FieldRequiredness Requiredness { get; }

        /// <summary>
        /// Gets the data type of the field.
        /// </summary>
        FieldType Type { get; }

        /// <summary>
        /// Gets a value indicating whether the field Id was generated implicitly,
        /// rather than being explicitly defined in the Thrift IDL.
        /// </summary>
        bool IsFieldIdImplicit { get; }
    }
}