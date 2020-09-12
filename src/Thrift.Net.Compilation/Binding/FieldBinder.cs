namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind fields based on a parse tree.
    /// </summary>
    public class FieldBinder : Binder<FieldContext, FieldDefinition>, IBinder
    {
        private readonly IFieldContainerBinder containerBinder;
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldBinder" /> class.
        /// </summary>
        /// <param name="containerBinder">The container binder.</param>
        /// <param name="binderProvider">Used to find binders for child nodes.</param>
        public FieldBinder(
            IFieldContainerBinder containerBinder, IBinderProvider binderProvider)
            : base(containerBinder)
        {
            this.containerBinder = containerBinder;
            this.binderProvider = binderProvider;
        }

        /// <summary>
        /// Binds the specified field.
        /// </summary>
        /// <param name="context">The parsed field.</param>
        /// <returns>The field definition.</returns>
        protected override FieldDefinition Bind(FieldContext context)
        {
            var fieldId = this.GetFieldId(context);
            var requiredness = this.GetFieldRequiredness(context);
            var typeBinder = this.binderProvider.GetBinder(context.fieldType());
            var type = typeBinder.Bind<FieldType>(context.fieldType());
            bool isFieldIdImplicit = context.fieldId == null;

            return new FieldDefinition(
                fieldId,
                context.fieldId?.Text,
                requiredness,
                type,
                context.name.Text,
                isFieldIdImplicit);
        }

        private FieldRequiredness GetFieldRequiredness(FieldContext context)
        {
            if (context.fieldRequiredness()?.REQUIRED() != null)
            {
                return FieldRequiredness.Required;
            }

            if (context.fieldRequiredness()?.OPTIONAL() != null)
            {
                return FieldRequiredness.Optional;
            }

            return this.containerBinder.DefaultFieldRequiredness;
        }

        private int? GetFieldId(FieldContext context)
        {
            if (context.fieldId != null)
            {
                if (int.TryParse(context.fieldId.Text, out var fieldId) &&
                    fieldId >= 0)
                {
                    return fieldId;
                }

                return null;
            }

            return this.containerBinder.GetAutomaticFieldId(context);
        }
    }
}