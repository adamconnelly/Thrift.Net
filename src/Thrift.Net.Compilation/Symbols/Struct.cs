namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// Represents a Thrift struct.
    /// </summary>
    public class Struct : Symbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Struct" /> class.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="name">The name of the struct.</param>
        /// <param name="fields">The struct's fields.</param>
        public Struct(IParseTree node, string name, IReadOnlyCollection<Field> fields)
            : base(node)
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