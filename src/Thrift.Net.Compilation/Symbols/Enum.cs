namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Describes an enum.
    /// </summary>
    public class Enum : NamedSymbol<EnumDefinitionContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enum" /> class.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="name">The name of the enum.</param>
        /// <param name="members">The enum members.</param>
        public Enum(EnumDefinitionContext node, string name, IReadOnlyCollection<EnumMember> members)
            : base(node, name)
        {
            this.Members = members;
        }

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