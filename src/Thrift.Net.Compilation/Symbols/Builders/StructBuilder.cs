namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Struct" /> objects.
    /// </summary>
    public class StructBuilder : SymbolBuilder<StructDefinitionContext, Struct, Document, StructBuilder>
    {
        /// <summary>
        /// Gets the name of the struct.
        /// </summary>
        public string Name { get; private set; }

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
        /// Builds the struct.
        /// </summary>
        /// <returns>The struct.</returns>
        public override Struct Build()
        {
            return new Struct(this.Node, this.Parent, this.Name, this.BinderProvider);
        }
    }
}