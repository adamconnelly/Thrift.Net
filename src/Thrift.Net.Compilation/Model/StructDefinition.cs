namespace Thrift.Net.Compilation.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Thrift struct.
    /// </summary>
    public class StructDefinition : ISymbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the struct.</param>
        /// <param name="fields">The struct's fields.</param>
        public StructDefinition(string name, IReadOnlyCollection<FieldDefinition> fields)
        {
            this.Name = name;
            this.Fields = fields;
        }

        /// <summary>
        /// Gets the name of the struct.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the fields of the struct.
        /// </summary>
        public IReadOnlyCollection<FieldDefinition> Fields { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"struct {this.Name}";
        }
    }
}