namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Represents a field in a struct, union or exception.
    /// </summary>
    public class Field : ISymbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Field" /> class.
        /// </summary>
        /// <param name="fieldId">The field's Id.</param>
        /// <param name="rawFieldId">The raw text representing the field Id.</param>
        /// <param name="requiredness">The level of requiredness of the field.</param>
        /// <param name="type">The type of the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="isFieldIdImplicit">
        /// Indicates whether the field Id was generated automatically by the
        /// compiler rather than being explicitly defined in the IDL.
        /// </param>
        public Field(
            int? fieldId,
            string rawFieldId,
            FieldRequiredness requiredness,
            FieldType type,
            string name,
            bool isFieldIdImplicit)
        {
            this.FieldId = fieldId;
            this.RawFieldId = rawFieldId;
            this.Requiredness = requiredness;
            this.Type = type;
            this.Name = name;
            this.IsFieldIdImplicit = isFieldIdImplicit;
        }

        /// <summary>
        /// Gets the field's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the Id of the field.
        /// </summary>
        public int? FieldId { get; }

        /// <summary>
        /// Gets the raw text of the field Id rather than the integer value
        /// stored in <see cref="FieldId" />.
        /// </summary>
        public string RawFieldId { get; }

        /// <summary>
        /// Gets the level of requiredness of this field.
        /// </summary>
        public FieldRequiredness Requiredness { get; }

        /// <summary>
        /// Gets the data type of the field.
        /// </summary>
        public FieldType Type { get; }

        /// <summary>
        /// Gets a value indicating whether the field Id was generated implicitly,
        /// rather than being explicitly defined in the Thrift IDL.
        /// </summary>
        public bool IsFieldIdImplicit { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.FieldId}: {this.Requiredness} {this.Type} {this.Name}";
        }
    }
}