namespace Thrift.Net.Compilation.Binding
{
    using System.Linq;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind fields based on a parse tree.
    /// </summary>
    public class FieldBinder : Binder<FieldContext, Field, IFieldContainer>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">Used to find binders for child nodes.</param>
        public FieldBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override Field Bind(FieldContext node, IFieldContainer parent)
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

            return FieldRequiredness.Default;
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

            if (node.Parent is StructDefinitionContext structNode)
            {
                return (structNode.field()
                    .Where(field => field.fieldId == null)
                    .ToList()
                    .IndexOf(node) + 1) * -1;
            }

            if (node.Parent is UnionDefinitionContext unionNode)
            {
                return (unionNode.field()
                    .Where(field => field.fieldId == null)
                    .ToList()
                    .IndexOf(node) + 1) * -1;
            }

            return -1;
        }
    }
}