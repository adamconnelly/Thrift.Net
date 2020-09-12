namespace Thrift.Net.Compilation.Binding
{
    using System;
    using System.Linq;
    using Thrift.Net.Compilation.Model;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind a <see cref="StructDefinition" /> from the parse tree.
    /// </summary>
    public class StructBinder : Binder<StructDefinitionContext, StructDefinition>, IBinder, IFieldContainerBinder
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructBinder" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        /// <param name="binderProvider">
        /// Used to get the correct binder for a particular node.
        /// </param>
        public StructBinder(IBinder parent, IBinderProvider binderProvider)
            : base(parent)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        public FieldRequiredness DefaultFieldRequiredness => FieldRequiredness.Default;

        /// <inheritdoc />
        public bool IsFieldNameAlreadyDefined(string name, FieldContext node)
        {
            var parentNode = node.Parent as StructDefinitionContext;

            return parentNode.field()
                .Select(node => new
                    {
                        Node = node,
                        Symbol = this.binderProvider.GetBinder(node).Bind<FieldDefinition>(node),
                    })
                .Where(item => item.Symbol.Name == name)
                .FirstOrDefault().Node != node;
        }

        /// <inheritdoc />
        public bool IsFieldIdAlreadyDefined(int fieldId, FieldContext node)
        {
            var parentNode = node.Parent as StructDefinitionContext;

            return parentNode.field()
                .Select(node => new
                    {
                        Node = node,
                        Symbol = this.binderProvider.GetBinder(node).Bind<FieldDefinition>(node),
                    })
                .Where(item => item.Symbol.FieldId == fieldId)
                .FirstOrDefault().Node != node;
        }

        /// <inheritdoc />
        public int GetAutomaticFieldId(FieldContext node)
        {
            if (node.fieldId != null)
            {
                throw new InvalidOperationException(
                    "Cannot generate an automatic field Id because the field has an explicit Id specified");
            }

            var structDefinition = node.Parent as StructDefinitionContext;

            return (structDefinition.field()
                .Where(field => field.fieldId == null)
                .ToList()
                .IndexOf(node) + 1) * -1;
        }

        /// <summary>
        /// Creates a <see cref="StructDefinition" /> based on the parse tree node.
        /// </summary>
        /// <param name="context">The node to bind.</param>
        /// <returns>The struct definition.</returns>
        protected override StructDefinition Bind(StructDefinitionContext context)
        {
            var fields = context.field().Select(this.GetField);

            return new StructDefinition(context.name?.Text, fields.ToList());
        }

        private FieldDefinition GetField(FieldContext context)
        {
            var binder = this.binderProvider.GetBinder(context);

            return binder.Bind<FieldDefinition>(context);
        }
    }
}