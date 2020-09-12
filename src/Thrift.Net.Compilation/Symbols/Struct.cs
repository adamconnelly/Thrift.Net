namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Thrift struct.
    /// </summary>
    public class Struct : ISymbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Struct" /> class.
        /// </summary>
        /// <param name="name">The name of the struct.</param>
        /// <param name="fields">The struct's fields.</param>
        public Struct(string name, IReadOnlyCollection<Field> fields)
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
        public IReadOnlyCollection<Field> Fields { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"struct {this.Name}";
        }
    }
}