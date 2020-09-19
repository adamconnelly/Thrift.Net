namespace Thrift.Net.Compilation.Symbols.Builders
{
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Struct" /> objects.
    /// </summary>
    public class StructBuilder : SymbolBuilder<StructDefinitionContext, Struct, Document, StructBuilder>
    {
        private readonly List<Field> fields = new List<Field>();

        /// <summary>
        /// Gets the name of the struct.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the fields of the struct.
        /// </summary>
        public IReadOnlyCollection<Field> Fields => this.fields;

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
        public override Struct Build()
        {
            return new Struct(
                this.Node,
                this.Parent,
                this.Name,
                this.Fields);
        }
    }
}