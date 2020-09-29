namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Describes an enum.
    /// </summary>
    public class Enum : NamedSymbol<EnumDefinitionContext>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Enum" /> class.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="parent">The document that contains this enum.</param>
        /// <param name="name">The name of the enum.</param>
        /// <param name="binderProvider">Used to get binders for nodes.</param>
        public Enum(
            EnumDefinitionContext node,
            Document parent,
            string name,
            IBinderProvider binderProvider)
            : base(node, parent, name)
        {
            this.binderProvider = binderProvider;
        }

        /// <summary>
        /// Gets the enum members.
        /// </summary>
        public IReadOnlyCollection<EnumMember> Members
        {
            get
            {
                return this.Node.enumMember()
                    .Select(memberNode => this.binderProvider
                        .GetBinder(memberNode)
                        .Bind<EnumMember>(memberNode, this))
                    .ToList();
            }
        }

        /// <inheritdoc />
        protected override IReadOnlyCollection<ISymbol> Children
        {
            get
            {
                return this.Members;
            }
        }

        /// <summary>
        /// Checks whether the specified name has already been used by another
        /// enum member.
        /// </summary>
        /// <param name="memberName">The name to check for.</param>
        /// <param name="node">The node being defined.</param>
        /// <returns>
        /// true if a member with the same name has already been declared, false otherwise.
        /// </returns>
        public bool IsEnumMemberAlreadyDeclared(string memberName, EnumMemberContext node)
        {
            var parent = node.Parent as EnumDefinitionContext;

            // If there's only a single member, don't bother doing any more work
            if (parent.enumMember().Length == 1)
            {
                return false;
            }

            var matchingMembers = this.Members
                .Where(member => member.Name == memberName)
                .TakeWhile(member => member.Node != node);

            return matchingMembers.Any();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"enum {this.Name}";
        }
    }
}