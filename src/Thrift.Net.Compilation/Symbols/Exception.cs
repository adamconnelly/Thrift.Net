namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift Exception.
    /// </summary>
    public class Exception : NamedSymbol<ExceptionDefinitionContext, IDocument>, IException
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception" /> class.
        /// </summary>
        /// <param name="node">The union parse tree node.</param>
        /// <param name="binderProvider">Used to get binders.</param>
        /// <param name="parent">The document that contains this union.</param>
        /// <param name="name">The name of the union.</param>
        public Exception(
            ExceptionDefinitionContext node,
            IBinderProvider binderProvider,
            IDocument parent,
            string name)
            : base(node, parent, name)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        public IDocument Document => this.Parent as IDocument;

        /// <inheritdoc/>
        public IReadOnlyCollection<Field> Fields
        {
            get
            {
                return this.Node.field()
                    .Select(fieldNode => this.binderProvider
                        .GetBinder(fieldNode)
                        .Bind<Field>(fieldNode, this))
                    .ToList();
            }
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Field> OptionalFields => this.Fields
            .Where(field => field.Requiredness != FieldRequiredness.Required)
            .ToList();

        /// <inheritdoc/>
        public IReadOnlyCollection<Field> RequiredFields => this.Fields
            .Where(field => field.Requiredness == FieldRequiredness.Required)
            .ToList();

        /// <inheritdoc/>
        protected override IReadOnlyCollection<ISymbol> Children => this.Fields;

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitException(this);
            base.Accept(visitor);
        }
    }
}