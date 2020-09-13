namespace Thrift.Net.Compilation.Symbols.Builders
{
    using System.Collections.Generic;
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// Used to build <see cref="Struct" /> objects.
    /// </summary>
    public class StructBuilder
    {
        private readonly List<Field> fields = new List<Field>();

        /// <summary>
        /// Gets the node associated with the struct.
        /// </summary>
        public IParseTree Node { get; private set; }

        /// <summary>
        /// Gets the name of the struct.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the fields of the struct.
        /// </summary>
        public IReadOnlyCollection<Field> Fields => this.fields;

        /// <summary>
        /// Sets the node associated with the struct.
        /// </summary>
        /// <param name="node">The node associated with the struct.</param>
        /// <returns>The builder.</returns>
        public StructBuilder SetNode(IParseTree node)
        {
            this.Node = node;

            return this;
        }

        /// <summary>
        /// Sets the name of the struct.
        /// </summary>
        /// <param name="name">The name of the struct.</param>
        /// <returns>The builder.</returns>
        public StructBuilder SetName(string name)
        {
            this.Name = name;

            return this;
        }

        /// <summary>
        /// Adds a field to the struct.
        /// </summary>
        /// <param name="field">The field to add.</param>
        /// <returns>The builder.</returns>
        public StructBuilder AddField(Field field)
        {
            this.fields.Add(field);

            return this;
        }

        /// <summary>
        /// Adds a collection of fields to the struct.
        /// </summary>
        /// <param name="fields">The fields to add.</param>
        /// <returns>The builder.</returns>
        public StructBuilder AddFields(IEnumerable<Field> fields)
        {
            foreach (var field in fields)
            {
                this.AddField(field);
            }

            return this;
        }

        /// <summary>
        /// Adds a field to the struct, using the specified action to configure
        /// the field.
        /// </summary>
        /// <param name="configureField">Used to configure the field.</param>
        /// <returns>The builder.</returns>
        public StructBuilder AddField(System.Action<FieldBuilder> configureField)
        {
            var builder = new FieldBuilder();
            configureField(builder);
            this.AddField(builder.Build());

            return this;
        }

        /// <summary>
        /// Builds the struct.
        /// </summary>
        /// <returns>The struct.</returns>
        public Struct Build()
        {
            return new Struct(this.Node, this.Name, this.Fields);
        }
    }
}