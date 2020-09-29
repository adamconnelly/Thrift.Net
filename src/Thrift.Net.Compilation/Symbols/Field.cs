namespace Thrift.Net.Compilation.Symbols
{
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a field in a struct, union or exception.
    /// </summary>
    public class Field : Symbol<FieldContext>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Field" /> class.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="parent">The struct that contains this field.</param>
        /// <param name="fieldId">The field's Id.</param>
        /// <param name="rawFieldId">The raw text representing the field Id.</param>
        /// <param name="requiredness">The level of requiredness of the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="isFieldIdImplicit">
        /// Indicates whether the field Id was generated automatically by the
        /// compiler rather than being explicitly defined in the IDL.
        /// </param>
        /// <param name="binderProvider">Used to get binders.</param>
        public Field(
            FieldContext node,
            Struct parent,
            int? fieldId,
            string rawFieldId,
            FieldRequiredness requiredness,
            string name,
            bool isFieldIdImplicit,
            IBinderProvider binderProvider)
            : base(node, parent)
        {
            this.FieldId = fieldId;
            this.RawFieldId = rawFieldId;
            this.Requiredness = requiredness;
            this.Name = name;
            this.IsFieldIdImplicit = isFieldIdImplicit;
            this.binderProvider = binderProvider;
        }

        /// <summary>
        /// Gets the field's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the Id of the field. This will be null if the field Id could
        /// not be parsed from the IDL, for example if it is negative or not
        /// an integer. The <see cref="RawFieldId" /> field can be used to get
        /// the raw string value.
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
        public FieldType Type
        {
            get
            {
                var typeBinder = this.binderProvider.GetBinder(this.Node.fieldType());
                return typeBinder.Bind<FieldType>(this.Node.fieldType(), this);
            }
        }

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