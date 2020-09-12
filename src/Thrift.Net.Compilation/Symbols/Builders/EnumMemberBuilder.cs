namespace Thrift.Net.Compilation.Symbols.Builders
{
    /// <summary>
    /// Used to build <see cref="EnumMember" /> objects.
    /// </summary>
    public class EnumMemberBuilder
    {
        /// <summary>
        /// Gets the name of the enum member.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the enum value.
        /// </summary>
        public int Value { get; private set; }

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
        public EnumMemberBuilder SetValue(int value)
        {
            this.Value = value;

            return this;
        }

        /// <summary>
        /// Builds the enum member.
        /// </summary>
        /// <returns>The enum member.</returns>
        public EnumMember Build()
        {
            return new EnumMember(this.Name, this.Value);
        }
    }
}