namespace Thrift.Net.Compilation.Model
{
    /// <summary>
    /// Represents an individual member of an enum.
    /// </summary>
    public class EnumMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMember" /> class.
        /// </summary>
        /// <param name="name">The name of the enum member.</param>
        /// <param name="value">The value of the enum member.</param>
        public EnumMember(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets the name of the enum member.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the enum value.
        /// </summary>
        public int Value { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Name} = {this.Value}";
        }
    }
}