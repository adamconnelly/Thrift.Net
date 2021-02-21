namespace Thrift.Net.Compilation.Symbols
{
    using Thrift.Net.Compilation.Types;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a constant expression value.
    /// </summary>
    public class ConstantExpression : Symbol<ConstExpressionContext, IConstant>, IConstantExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantExpression" /> class.
        /// </summary>
        /// <param name="node">The parse tree node.</param>
        /// <param name="parent">The constant that contains this value.</param>
        /// <param name="rawValue">The raw value representing the constant expression.</param>
        /// <param name="type">The type of the constant expression.</param>
        public ConstantExpression(
            ConstExpressionContext node,
            IConstant parent,
            string rawValue,
            IType type)
            : base(node, parent)
        {
            this.RawValue = rawValue;
            this.Type = type;
        }

        /// <inheritdoc/>
        public IType Type { get; }

        /// <inheritdoc/>
        public string RawValue { get; }
    }
}