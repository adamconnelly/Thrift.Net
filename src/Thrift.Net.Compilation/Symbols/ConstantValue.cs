namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a constant expression value.
    /// </summary>
    public class ConstantValue : Symbol<ConstExpressionContext, IConstant>, IConstantValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantValue" /> class.
        /// </summary>
        /// <param name="node">The parse tree node.</param>
        /// <param name="parent">The constant that contains this value.</param>
        /// <param name="rawValue">The raw value representing the constant expression.</param>
        /// <param name="type">The type of the constant expression.</param>
        public ConstantValue(
            ConstExpressionContext node,
            IConstant parent,
            string rawValue,
            IFieldType type)
            : base(node, parent)
        {
            this.RawValue = rawValue;
            this.Type = type;
        }

        /// <inheritdoc/>
        public IFieldType Type { get; }

        /// <inheritdoc/>
        public string RawValue { get; }
    }
}