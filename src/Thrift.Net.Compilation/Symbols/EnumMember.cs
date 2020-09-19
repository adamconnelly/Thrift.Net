namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents an individual member of an enum.
    /// </summary>
    public class EnumMember : Symbol<EnumMemberContext>
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
            Enum parent,
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

        /// <summary>
        /// Gets the name of the enum member.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the enum value.
        /// </summary>
        public int? Value { get; }

        /// <summary>
        /// Gets the raw text that represents the value of the enum.
        /// </summary>
        public string RawValue { get; }

        /// <summary>
        /// Gets the reason (if any) that the enum value has failed to be parsed.
        /// </summary>
        public InvalidEnumValueReason InvalidValueReason { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Name} = {this.Value}";
        }
    }
}