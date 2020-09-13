namespace Thrift.Net.Compilation.Symbols.Builders
{
    using System.Collections.Generic;
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// A builder that can be used to create <see cref="Enum" /> objects.
    /// </summary>
    public class EnumBuilder
    {
        private readonly List<EnumMember> members = new List<EnumMember>();

        /// <summary>
        /// Gets the tree node associated with the enum.
        /// </summary>
        public IParseTree Node { get; private set; }

        /// <summary>
        /// Gets the name of the enum.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the enum members.
        /// </summary>
        public IReadOnlyCollection<EnumMember> Members => this.members;

        /// <summary>
        /// Sets the node associated with the enum.
        /// </summary>
        /// <param name="node">The node associated with the enum.</param>
        /// <returns>The builder.</returns>
        public EnumBuilder SetNode(IParseTree node)
        {
            this.Node = node;

            return this;
        }

        /// <summary>
        /// Sets the name of the enum.
        /// </summary>
        /// <param name="name">The name of the enum.</param>
        /// <returns>The builder.</returns>
        public EnumBuilder SetName(string name)
        {
            this.Name = name;

            return this;
        }

        /// <summary>
        /// Adds a new member to the enum.
        /// </summary>
        /// <param name="member">The member to add.</param>
        /// <returns>The builder.</returns>
        public EnumBuilder AddMember(EnumMember member)
        {
            this.members.Add(member);

            return this;
        }

        /// <summary>
        /// Adds a new member to the builder, using the specified action to
        /// configure the member.
        /// </summary>
        /// <param name="configureMember">Used to configure the enum member.</param>
        /// <returns>The builder.</returns>
        public EnumBuilder AddMember(System.Action<EnumMemberBuilder> configureMember)
        {
            var builder = new EnumMemberBuilder();
            configureMember(builder);
            this.AddMember(builder.Build());

            return this;
        }

        /// <summary>
        /// Adds the specified members to the builder.
        /// </summary>
        /// <param name="members">Adds the members to the enum.</param>
        /// <returns>The builder.</returns>
        public EnumBuilder AddMembers(IEnumerable<EnumMember> members)
        {
            foreach (var member in members)
            {
                this.AddMember(member);
            }

            return this;
        }

        /// <summary>
        /// Builds the enum.
        /// </summary>
        /// <returns>The enum.</returns>
        public Enum Build()
        {
            return new Enum(this.Node, this.Name, this.Members);
        }
    }
}