namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift struct.
    /// </summary>
    public class Struct : NamedSymbol<StructDefinitionContext>
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
            Document parent,
            string name,
            IBinderProvider binderProvider)
            : base(node, parent, name)
        {
            this.binderProvider = binderProvider;
        }

        /// <summary>
        /// Gets the fields of the struct.
        /// </summary>
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

        /// <summary>
        /// Gets the fields that are optional (either explicitly or implicitly).
        /// </summary>
        public IReadOnlyCollection<Field> OptionalFields => this.Fields
            .Where(field => field.Requiredness != FieldRequiredness.Required)
            .ToList();

        /// <inheritdoc />
        protected override IReadOnlyCollection<ISymbol> Children
        {
            get
            {
                return this.Fields;
            }
        }

        /// <summary>
        /// Checks whether the field has already been defined.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="node">The field being defined.</param>
        /// <returns>
        /// true if the field name has already been defined, false otherwise.
        /// </returns>
        public bool IsFieldNameAlreadyDefined(string name, FieldContext node)
        {
            return this.Fields
                .Where(item => item.Name == name)
                .FirstOrDefault().Node != node;
        }

        /// <summary>
        /// Checks whether a field with the specified Id has already been defined.
        /// </summary>
        /// <param name="fieldId">The field Id to check for.</param>
        /// <param name="node">The field being defined.</param>
        /// <returns>
        /// true if the field Id has already been defined, false otherwise.
        /// </returns>
        public bool IsFieldIdAlreadyDefined(int fieldId, FieldContext node)
        {
            return this.Fields
                .Where(item => item.FieldId == fieldId)
                .FirstOrDefault().Node != node;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"struct {this.Name}";
        }
    }
}