namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind the type of fields.
    /// </summary>
    public class FieldTypeBinder : Binder<FieldTypeContext, IFieldType, IField>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldTypeBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">Used to get binders for nodes.</param>
        public FieldTypeBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override IFieldType Bind(FieldTypeContext node, IField parent)
        {
            if (node.baseType() != null)
            {
                return this.binderProvider
                    .GetBinder(node.baseType())
                    .Bind<IFieldType>(node.baseType(), parent);
            }

            return this.binderProvider
                .GetBinder(node.userType())
                .Bind<IFieldType>(node.userType(), parent);
        }
    }
}