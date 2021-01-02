namespace Thrift.Net.Compilation.Symbols.Builders
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Constant" /> objects.
    /// </summary>
    public class ConstantValueBuilder : SymbolBuilder<ConstExpressionContext, ConstantValue, IConstant, ConstantValueBuilder>
    {
        private string rawValue;
        private IFieldType type;

        /// <summary>
        /// Gets the raw string representing the constant expression.
        /// </summary>
        public string RawValue => this.rawValue;

        /// <summary>
        /// Gets the type of the constant expression.
        /// </summary>
        public IFieldType Type => this.type;

        /// <summary>
        /// Sets the raw string representing the constant expression.
        /// </summary>
        /// <param name="rawValue">The string representation of the constant expression.</param>
        /// <returns>The builder.</returns>
        public ConstantValueBuilder SetRawValue(string rawValue)
        {
            this.rawValue = rawValue;

            return this;
        }

        /// <summary>
        /// Sets the type of the constant expression.
        /// </summary>
        /// <param name="type">The type of the expression.</param>
        /// <returns>The builder.</returns>
        public ConstantValueBuilder SetType(IFieldType type)
        {
            this.type = type;

            return this;
        }

        /// <inheritdoc/>
        public override ConstantValue Build()
        {
            return new ConstantValue(this.Node, this.Parent, this.RawValue, this.Type);
        }
    }
}