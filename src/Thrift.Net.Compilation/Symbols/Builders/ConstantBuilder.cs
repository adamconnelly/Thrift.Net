namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Constant" /> objects.
    /// </summary>
    public class ConstantBuilder : SymbolBuilder<ConstDefinitionContext, Constant, IDocument, ConstantBuilder>
    {
        private string name;

        /// <summary>
        /// Gets the name of the constant.
        /// </summary>
        public string Name => this.name;

        /// <summary>
        /// Sets the name of the constant.
        /// </summary>
        /// <param name="name">The name of the constant.</param>
        /// <returns>The builder.</returns>
        public ConstantBuilder SetName(string name)
        {
            this.name = name;

            return this;
        }

        /// <inheritdoc/>
        public override Constant Build()
        {
            return new Constant(this.Node, this.Parent, this.Name, this.BinderProvider);
        }
    }
}