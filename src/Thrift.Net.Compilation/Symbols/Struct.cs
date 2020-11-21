namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift struct.
    /// </summary>
    public class Struct : NamedSymbol<StructDefinitionContext, IDocument>, IStruct
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Struct" /> class.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="parent">The document containing this struct.</param>
        /// <param name="name">The name of the struct.</param>
        /// <param name="binderProvider">Used to get binders for nodes.</param>
        public Struct(
            StructDefinitionContext node,
            IDocument parent,
            string name,
            IBinderProvider binderProvider)
            : base(node, parent, name)
        {
            this.binderProvider = binderProvider;
        }

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
        public IDocument Document => this.Parent;

        /// <inheritdoc />
        protected override IReadOnlyCollection<ISymbol> Children
        {
            get
            {
                return this.Fields;
            }
        }

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitStruct(this);
            base.Accept(visitor);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"struct {this.Name}";
        }
    }
}