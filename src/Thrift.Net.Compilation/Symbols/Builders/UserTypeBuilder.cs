namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Builds <see cref="UserType" /> objects.
    /// </summary>
    public class UserTypeBuilder : SymbolBuilder<UserTypeContext, UserType, ISymbol, UserTypeBuilder>
    {
        private INamedTypeSymbol definition;

        /// <summary>
        /// Gets the symbol that represents the definition of the type.
        /// </summary>
        public INamedTypeSymbol Definition => this.definition;

        /// <summary>
        /// Sets the symbol that represents the definition of the type.
        /// </summary>
        /// <param name="definition">The definition of the type.</param>
        /// <returns>The builder.</returns>
        public UserTypeBuilder SetDefinition(INamedTypeSymbol definition)
        {
            this.definition = definition;

            return this;
        }

        /// <inheritdoc/>
        public override UserType Build()
        {
            return new UserType(this.Node, this.Parent, this.Definition);
        }
    }
}