namespace Thrift.Net.Compilation.Binding
{
    using System.Linq;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind fields based on a parse tree.
    /// </summary>
    public class FieldBinder : Binder<FieldContext, Field, IStruct>
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
        {
            this.containerBinder = containerBinder;
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override Field Bind(FieldContext node, IStruct parent)
        {
            var builder = new FieldBuilder()
                .SetNode(node)
                .SetParent(parent)
                .SetBinderProvider(this.binderProvider)
                .SetFieldId(this.GetFieldId(node))
                .SetIsFieldIdImplicit(node.fieldId == null)
                .SetRawFieldId(node.fieldId?.Text)
                .SetRequiredness(this.GetFieldRequiredness(node))
                .SetName(node.name.Text);

            return builder.Build();
        }

        private FieldRequiredness GetFieldRequiredness(FieldContext node)
        {
            if (node.fieldRequiredness()?.REQUIRED() != null)
            {
                return FieldRequiredness.Required;
            }

            if (node.fieldRequiredness()?.OPTIONAL() != null)
            {
                return FieldRequiredness.Optional;
            }

            return this.containerBinder.DefaultFieldRequiredness;
        }

        private int? GetFieldId(FieldContext node)
        {
            if (node.fieldId != null)
            {
                if (int.TryParse(node.fieldId.Text, out var fieldId) &&
                    fieldId > 0)
                {
                    return fieldId;
                }

                return null;
            }

            if (!(node.Parent is StructDefinitionContext parentNode))
            {
                return -1;
            }

            return (parentNode.field()
                .Where(field => field.fieldId == null)
                .ToList()
                .IndexOf(node) + 1) * -1;
        }
    }
}