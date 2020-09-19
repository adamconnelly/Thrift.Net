namespace Thrift.Net.Compilation.Symbols.Builders
{
    using Antlr4.Runtime.Tree;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Field" /> objects.
    /// </summary>
    public class FieldBuilder : SymbolBuilder<FieldContext, Field, Struct, FieldBuilder>
    {
        /// <summary>
        /// Gets the field's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the Id of the field.
        /// </summary>
        public int? FieldId { get; private set; }

        /// <summary>
        /// Gets the raw text of the field Id rather than the integer value
        /// stored in <see cref="FieldId" />.
        /// </summary>
        public string RawFieldId { get; private set; }

        /// <summary>
        /// Gets the level of requiredness of this field.
        /// </summary>
        public FieldRequiredness Requiredness { get; private set; }

        /// <summary>
        /// Gets the data type of the field.
        /// </summary>
        public FieldType Type { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the field Id was generated implicitly,
        /// rather than being explicitly defined in the Thrift IDL.
        /// </summary>
        public bool IsFieldIdImplicit { get; private set; }

        /// <summary>
        /// Sets the name of the field.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <returns>The field builder.</returns>
        public FieldBuilder SetName(string name)
        {
            this.Name = name;

            return this;
        }

        /// <summary>
        /// Sets the field Id. This will either be explicit (i.e. defined in the IDL)
        /// or implicit (i.e. assigned automatically by the compiler).
        /// </summary>
        /// <param name="fieldId">The field Id.</param>
        /// <returns>The field builder.</returns>
        public FieldBuilder SetFieldId(int? fieldId)
        {
            this.FieldId = fieldId;

            return this;
        }

        /// <summary>
        /// Sets the raw field Id.
        /// </summary>
        /// <param name="rawFieldId">
        /// The raw field Id (i.e. exactly what was defined in the IDL).
        /// </param>
        /// <returns>The field builder.</returns>
        public FieldBuilder SetRawFieldId(string rawFieldId)
        {
            this.RawFieldId = rawFieldId;

            return this;
        }

        /// <summary>
        /// Sets the requiredness of the field.
        /// </summary>
        /// <param name="requiredness">The requiredness of the field.</param>
        /// <returns>The field builder.</returns>
        public FieldBuilder SetRequiredness(FieldRequiredness requiredness)
        {
            this.Requiredness = requiredness;

            return this;
        }

        /// <summary>
        /// Sets the field's type.
        /// </summary>
        /// <param name="type">The field's type.</param>
        /// <returns>The field builder.</returns>
        public FieldBuilder SetType(FieldType type)
        {
            this.Type = type;

            return this;
        }

        /// <summary>
        /// Sets whether the field Id is implicit (i.e. not defined in the IDL).
        /// </summary>
        /// <param name="isImplicit">Indicates whether the field is implicit.</param>
        /// <returns>The field builder.</returns>
        public FieldBuilder SetIsFieldIdImplicit(bool isImplicit)
        {
            this.IsFieldIdImplicit = isImplicit;

            return this;
        }

        /// <summary>
        /// Builds the field.
        /// </summary>
        /// <returns>The field.</returns>
        public override Field Build()
        {
            return new Field(
                this.Node,
                this.Parent,
                this.FieldId,
                this.RawFieldId,
                this.Requiredness,
                this.Type,
                this.Name,
                this.IsFieldIdImplicit);
        }
    }
}