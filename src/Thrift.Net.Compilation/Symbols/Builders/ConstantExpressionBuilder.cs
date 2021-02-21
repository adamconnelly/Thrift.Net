namespace Thrift.Net.Compilation.Symbols.Builders
{
    using Thrift.Net.Compilation.Types;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Constant" /> objects.
    /// </summary>
    public class ConstantExpressionBuilder : SymbolBuilder<ConstExpressionContext, ConstantExpression, IConstant, ConstantExpressionBuilder>
    {
        private string rawValue;
        private IType type;

        /// <summary>
        /// Gets the raw string representing the constant expression.
        /// </summary>
        public string RawValue => this.rawValue;

        /// <summary>
        /// Gets the type of the constant expression.
        /// </summary>
        public IType Type => this.type;

        /// <summary>
        /// Sets the raw string representing the constant expression.
        /// </summary>
        /// <param name="rawValue">The string representation of the constant expression.</param>
        /// <returns>The builder.</returns>
        public ConstantExpressionBuilder SetRawValue(string rawValue)
        {
            this.rawValue = rawValue;

            return this;
        }

        /// <summary>
        /// Sets the type of the constant expression.
        /// </summary>
        /// <param name="type">The type of the expression.</param>
        /// <returns>The builder.</returns>
        public ConstantExpressionBuilder SetType(IType type)
        {
            this.type = type;

            return this;
        }

        /// <inheritdoc/>
        public override ConstantExpression Build()
        {
            return new ConstantExpression(this.Node, this.Parent, this.RawValue, this.Type);
        }
    }
}