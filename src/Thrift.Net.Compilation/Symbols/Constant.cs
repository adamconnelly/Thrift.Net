namespace Thrift.Net.Compilation.Symbols
{
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift constant definition.
    /// </summary>
    public class Constant : NamedSymbol<ConstDefinitionContext, IDocument>, IConstant
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Constant" /> class.
        /// </summary>
        /// <param name="node">The parse tree node this symbol is bound to.</param>
        /// <param name="parent">The document containing the constant.</param>
        /// <param name="name">The name of the constant.</param>
        /// <param name="binderProvider">Used to get binders.</param>
        public Constant(
            ConstDefinitionContext node,
            IDocument parent,
            string name,
            IBinderProvider binderProvider)
            : base(node, parent, name)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        public IFieldType Type
        {
            get
            {
                return this.binderProvider.GetBinder(this.Node.fieldType())
                    .Bind<IFieldType>(this.Node.fieldType(), this);
            }
        }

        /// <inheritdoc/>
        public IConstantExpression Value
        {
            get
            {
                return this.binderProvider.GetBinder(this.Node.constExpression())
                    .Bind<IConstantExpression>(this.Node.constExpression(), this);
            }
        }

        /// <inheritdoc/>
        public IDocument Document => this.Document;
    }
}
