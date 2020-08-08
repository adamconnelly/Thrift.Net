namespace Thrift.Net.Compilation.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes an enum.
    /// </summary>
    public class EnumDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the enum.</param>
        /// <param name="members">The enum members.</param>
        public EnumDefinition(string name, IReadOnlyCollection<EnumMember> members)
        {
            this.Name = name;
            this.Members = members;
        }

        /// <summary>
        /// Gets the name of the enum.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the enum members.
        /// </summary>
        public IReadOnlyCollection<EnumMember> Members { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"enum {this.Name}";
        }
    }
}