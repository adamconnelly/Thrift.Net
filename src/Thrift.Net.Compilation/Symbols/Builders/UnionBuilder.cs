namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Union" /> symbols.
    /// </summary>
    public class UnionBuilder : SymbolBuilder<UnionDefinitionContext, Union, IDocument, UnionBuilder>
    {
        /// <summary>
        /// Gets the name of the union.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Sets the name of the union.
        /// </summary>
        /// <param name="name">The name of the union.</param>
        /// <returns>The builder.</returns>
        public UnionBuilder SetName(string name)
        {
            this.Name = name;

            return this;
        }

        /// <inheritdoc/>
        public override Union Build()
        {
            return new Union(this.Node, this.BinderProvider, this.Parent, this.Name);
        }
    }
}