namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift struct.
    /// </summary>
    public class Struct : NamedSymbol<StructDefinitionContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Struct" /> class.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="name">The name of the struct.</param>
        /// <param name="fields">The struct's fields.</param>
        public Struct(StructDefinitionContext node, string name, IReadOnlyCollection<Field> fields)
            : base(node, name)
        {
            this.Fields = fields;
        }

        /// <summary>
        /// Gets the fields of the struct.
        /// </summary>
        public IReadOnlyCollection<Field> Fields { get; }

        /// <summary>
        /// Gets the fields that are optional (either explicitly or implicitly).
        /// </summary>
        public IReadOnlyCollection<Field> OptionalFields => this.Fields
            .Where(field => field.Requiredness != FieldRequiredness.Required)
            .ToList();

        /// <inheritdoc />
        public override string ToString()
        {
            return $"struct {this.Name}";
        }
    }
}