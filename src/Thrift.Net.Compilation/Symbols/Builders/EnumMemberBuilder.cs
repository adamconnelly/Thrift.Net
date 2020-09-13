namespace Thrift.Net.Compilation.Symbols.Builders
{
    using Antlr4.Runtime.Tree;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="EnumMember" /> objects.
    /// </summary>
    public class EnumMemberBuilder
    {
        /// <summary>
        /// Gets the node associated with the enum member.
        /// </summary>
        public EnumMemberContext Node { get; private set; }

        /// <summary>
        /// Gets the name of the enum member.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the enum value.
        /// </summary>
        public int? Value { get; private set; }

        /// <summary>
        /// Gets the raw text representation of the enum value.
        /// </summary>
        public string RawValue { get; private set; }

        /// <summary>
        /// Gets the reason the enum value is invalid.
        /// </summary>
        public InvalidEnumValueReason InvalidValueReason { get; private set; }

        /// <summary>
        /// Sets the node associated with the enum member.
        /// </summary>
        /// <param name="node">The node associated with the enum member.</param>
        /// <returns>The builder.</returns>
        public EnumMemberBuilder SetNode(EnumMemberContext node)
        {
            this.Node = node;

            return this;
        }

        /// <summary>
        /// Sets the name of the enum member.
        /// </summary>
        /// <param name="name">The name of the enum member.</param>
        /// <returns>The builder.</returns>
        public EnumMemberBuilder SetName(string name)
        {
            this.Name = name;

            return this;
        }

        /// <summary>
        /// Sets the value of the enum member.
        /// </summary>
        /// <param name="value">The value of the enum member.</param>
        /// <returns>The builder.</returns>
        public EnumMemberBuilder SetValue(int? value)
        {
            this.Value = value;

            return this;
        }

        /// <summary>
        /// Sets the raw text representation of the enum value.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        /// <returns>The builder.</returns>
        public EnumMemberBuilder SetRawValue(string rawValue)
        {
            this.RawValue = rawValue;

            return this;
        }

        /// <summary>
        /// Sets the reason the enum value is invalid.
        /// </summary>
        /// <param name="invalidValueReason">The reason the value is invalid.</param>
        /// <returns>The builder.</returns>
        public EnumMemberBuilder SetInvalidValueReason(InvalidEnumValueReason invalidValueReason)
        {
            this.InvalidValueReason = invalidValueReason;

            return this;
        }

        /// <summary>
        /// Builds the enum member.
        /// </summary>
        /// <returns>The enum member.</returns>
        public EnumMember Build()
        {
            return new EnumMember(
                this.Node,
                this.Name,
                this.Value,
                this.RawValue,
                this.InvalidValueReason);
        }
    }
}