namespace Thrift.Net.Compilation.Binding
{
    using System.Linq;
    using Thrift.Net.Compilation.Symbols;
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
            // TODO: Reimplement and add unit tests
            var parent = node.Parent as EnumDefinitionContext;
            EnumMemberContext previousMemberNode = null;

            foreach (var memberNode in parent.enumMember())
            {
                if (memberNode == node)
                {
                    break;
                }

                previousMemberNode = memberNode;
            }

            if (previousMemberNode != null)
            {
                var previousMember = this.binderProvider
                    .GetBinder(previousMemberNode)
                    .Bind<EnumMember>(previousMemberNode);

                if (previousMember.Value != null)
                {
                    return previousMember.Value.Value + 1;
                }
            }

            return 0;
        }

        /// <inheritdoc />
        protected override Enum Bind(EnumDefinitionContext node)
        {
            throw new System.NotImplementedException();
        }
    }
}