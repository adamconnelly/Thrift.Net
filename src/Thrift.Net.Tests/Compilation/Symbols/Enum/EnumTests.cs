namespace Thrift.Net.Tests.Compilation.Symbols.Enum
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using static Thrift.Net.Antlr.ThriftParser;

    public abstract class EnumTests
    {
        private readonly IBinder enumMemberBinder = Substitute.For<IBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();

        public IBinder EnumMemberBinder => this.enumMemberBinder;
        public IBinderProvider BinderProvider => this.binderProvider;

        protected Enum CreateEnumFromInput(string input)
        {
            var enumNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumDefinition());

            return new EnumBuilder()
                .SetNode(enumNode)
                .SetBinderProvider(this.BinderProvider)
                .Build();
        }

        protected EnumMember SetupMember(EnumMemberContext enumMemberNode, Enum containingEnum, string name = null, int? value = null)
        {
            var member = new EnumMemberBuilder()
                .SetNode(enumMemberNode)
                .SetName(name)
                .SetValue(value)
                .SetRawValue(value?.ToString())
                .Build();

            this.BinderProvider.GetBinder(enumMemberNode).Returns(this.enumMemberBinder);
            this.enumMemberBinder.Bind<EnumMember>(enumMemberNode, containingEnum).Returns(member);

            return member;
        }
    }
}