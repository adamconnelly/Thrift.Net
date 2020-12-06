namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Exception" /> symbols.
    /// </summary>
    public class ExceptionBuilder : SymbolBuilder<ExceptionDefinitionContext, Exception, IDocument, ExceptionBuilder>
    {
        /// <summary>
        /// Gets the name of the exception.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Sets the name of the exception.
        /// </summary>
        /// <param name="name">The name of the exception.</param>
        /// <returns>The builder.</returns>
        public ExceptionBuilder SetName(string name)
        {
            this.Name = name;

            return this;
        }

        /// <inheritdoc/>
        public override Exception Build()
        {
            return new Exception(this.Node, this.BinderProvider, this.Parent, this.Name);
        }
    }
}