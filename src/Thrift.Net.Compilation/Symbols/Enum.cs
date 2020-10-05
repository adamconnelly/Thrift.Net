namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents an enum.
    /// </summary>
    public class Enum : NamedSymbol<EnumDefinitionContext, IDocument>, IEnum
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
            IDocument parent,
            string name,
            IBinderProvider binderProvider)
            : base(node, parent, name)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<IEnumMember> Members
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

        /// <inheritdoc/>
        public bool IsEnumMemberAlreadyDeclared(IEnumMember enumMember)
        {
            // If there's only a single member, don't bother doing any more work
            if (this.Members.Count == 1)
            {
                return false;
            }

            var matchingMembers = this.Members
                .Where(member => member.Name == enumMember.Name)
                .TakeWhile(member => member.Node != enumMember.Node);

            return matchingMembers.Any();
        }

        /// <inheritdoc/>
        public bool IsEnumValueAlreadyDeclared(IEnumMember enumMember)
        {
            // If the member has no value (i.e. its value was invalid for some reason)
            // don't bother doing any more work.
            if (enumMember.Value == null)
            {
                return false;
            }

            // If there's only a single member, don't bother doing any more work
            if (this.Members.Count == 1)
            {
                return false;
            }

            var matchingMembers = this.Members
                .Where(member => member.Value == enumMember.Value)
                .TakeWhile(member => member.Node != enumMember.Node);

            return matchingMembers.Any();
        }

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitEnum(this);
            base.Accept(visitor);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"enum {this.Name}";
        }
    }
}