namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents an individual member of an enum.
    /// </summary>
    public class EnumMember : Symbol<EnumMemberContext>, IEnumMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMember" /> class.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="parent">The enum that contains this member.</param>
        /// <param name="name">The name of the enum member.</param>
        /// <param name="value">The value of the enum member.</param>
        /// <param name="rawValue">The raw text representation of the value.</param>
        /// <param name="invalidValueReason">The reason the enum value failed to parse.</param>
        public EnumMember(
            EnumMemberContext node,
            IEnum parent,
            string name,
            int? value,
            string rawValue,
            InvalidEnumValueReason invalidValueReason)
            : base(node, parent)
        {
            this.Name = name;
            this.Value = value;
            this.RawValue = rawValue;
            this.InvalidValueReason = invalidValueReason;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public int? Value { get; }

        /// <inheritdoc/>
        public string RawValue { get; }

        /// <inheritdoc/>
        public InvalidEnumValueReason InvalidValueReason { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Name} = {this.Value}";
        }
    }
}