namespace Thrift.Net.Compilation.Symbols
{
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a field in a struct, union or exception.
    /// </summary>
    public class Field : Symbol<FieldContext, IStruct>, IField
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
            IStruct parent,
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

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public int? FieldId { get; }

        /// <inheritdoc/>
        public string RawFieldId { get; }

        /// <inheritdoc/>
        public FieldRequiredness Requiredness { get; }

        /// <inheritdoc/>
        public bool IsRequired
        {
            get
            {
                return this.Requiredness == FieldRequiredness.Required;
            }
        }

        /// <inheritdoc/>
        public FieldType Type
        {
            get
            {
                var typeBinder = this.binderProvider.GetBinder(this.Node.fieldType());
                return typeBinder.Bind<FieldType>(this.Node.fieldType(), this);
            }
        }

        /// <inheritdoc/>
        public bool IsFieldIdImplicit { get; }

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitField(this);
            base.Accept(visitor);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.FieldId}: {this.Requiredness} {this.Type} {this.Name}";
        }
    }
}