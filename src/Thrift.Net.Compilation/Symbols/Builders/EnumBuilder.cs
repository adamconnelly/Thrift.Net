namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// A builder that can be used to create <see cref="Enum" /> objects.
    /// </summary>
    public class EnumBuilder : SymbolBuilder<EnumDefinitionContext, Enum, Document, EnumBuilder>
    {
        /// <summary>
        /// Gets the name of the enum.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Sets the name of the enum.
        /// </summary>
        /// <param name="name">The name of the enum.</param>
        /// <returns>The builder.</returns>
        public EnumBuilder SetName(string name)
        {
            this.Name = name;

            return this;
        }

        /// <summary>
        /// Builds the enum.
        /// </summary>
        /// <returns>The enum.</returns>
        public override Enum Build()
        {
            return new Enum(this.Node, this.Parent, this.Name, this.BinderProvider);
        }
    }
}