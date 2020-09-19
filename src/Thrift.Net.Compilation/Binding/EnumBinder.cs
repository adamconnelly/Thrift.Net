namespace Thrift.Net.Compilation.Binding
{
    using System.Linq;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="EnumDefinitionContext" /> objects to
    /// <see cref="Enum" /> objects.
    /// </summary>
    public class EnumBinder : Binder<EnumDefinitionContext, Enum>, IEnumBinder
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        /// <param name="binderProvider">The binder provider.</param>
        public EnumBinder(IBinder parent, IBinderProvider binderProvider)
            : base(parent)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        public int GetEnumValue(EnumMemberContext node)
        {
            var parent = node.Parent as EnumDefinitionContext;

            // If there's only a single member, don't bother doing any more work
            if (parent.enumMember().Length == 1)
            {
                return 0;
            }

            var members = parent.enumMember()
                .TakeWhile(memberNode => memberNode != node)
                .Select(memberNode => this.binderProvider
                    .GetBinder(memberNode)
                    .Bind<EnumMember>(memberNode))
                .Where(member => member.Value != null);

            if (members.Any())
            {
                return members.Last().Value.Value + 1;
            }

            return 0;
        }

        /// <inheritdoc />
        public bool IsEnumMemberAlreadyDeclared(string memberName, EnumMemberContext node)
        {
            var parent = node.Parent as EnumDefinitionContext;

            // If there's only a single member, don't bother doing any more work
            if (parent.enumMember().Length == 1)
            {
                return false;
            }

            var matchingMembers = parent.enumMember()
                .Select(memberNode => this.binderProvider
                    .GetBinder(memberNode)
                    .Bind<EnumMember>(memberNode))
                .Where(member => member.Name == memberName)
                .TakeWhile(member => member.Node != node);

            return matchingMembers.Any();
        }

        /// <inheritdoc />
        protected override Enum Bind(EnumDefinitionContext node)
        {
            var members = node.enumMember()
                .Select(memberNode => this.binderProvider
                    .GetBinder(memberNode)
                    .Bind<EnumMember>(memberNode));

            var builder = new EnumBuilder()
                .SetNode(node)
                .SetBinderProvider(this.binderProvider)
                .SetName(node.name?.Text)
                .AddMembers(members);

            return builder.Build();
        }
    }
}