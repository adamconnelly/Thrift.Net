namespace Thrift.Net.Tests.Compilation.Binding.EnumBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class MemberTests : EnumBinderTests
    {
        [Fact]
        public void MembersSupplied_UsesBinderToCreateMembers()
        {
            // Arrange
            var input =
@"enum UserType {
    User,
    Administrator
}";
            var enumNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumDefinition());

            var memberBinder = Substitute.For<IBinder>();
            this.BinderProvider.GetBinder(default).ReturnsForAnyArgs(memberBinder);

            var userMember = new EnumMemberBuilder().Build();
            memberBinder.Bind<EnumMember>(enumNode.enumMember()[0])
                .Returns(userMember);
            var administratorMember = new EnumMemberBuilder().Build();
            memberBinder.Bind<EnumMember>(enumNode.enumMember()[1])
                .Returns(administratorMember);

            // Act
            var symbol = this.Binder.Bind<Enum>(enumNode);

            // Assert
            Assert.Collection(
                symbol.Members,
                user => Assert.Same(userMember, user),
                administrator => Assert.Same(administratorMember, administrator));
        }
    }
}