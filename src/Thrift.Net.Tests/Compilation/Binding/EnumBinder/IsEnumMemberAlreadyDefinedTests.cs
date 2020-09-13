namespace Thrift.Net.Tests.Compilation.Binding.EnumBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;
    using static Thrift.Net.Antlr.ThriftParser;

    public class IsEnumMemberAlreadyDefinedTests : EnumBinderTests
    {
        private readonly IBinder enumMemberBinder = Substitute.For<IBinder>();

        [Fact]
        public void SingleMember_ReturnsFalse()
        {
            // Arrange
            var input =
@"enum UserType {
    User
}";
            var enumNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumDefinition());

            this.SetupMember(enumNode.enumMember()[0], "User");

            // Act
            var isAlreadyDefined = this.Binder.IsEnumMemberAlreadyDefined(
                "User", enumNode.enumMember()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void MemberAlreadyDefined_ReturnsTrue()
        {
            // Arrange
            var input =
@"enum UserType {
    User,
    User
}";
            var enumNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumDefinition());

            this.SetupMember(enumNode.enumMember()[0], "User");
            this.SetupMember(enumNode.enumMember()[1], "User");

            // Act
            var isAlreadyDefined = this.Binder.IsEnumMemberAlreadyDefined(
                "User", enumNode.enumMember()[1]);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void MemberDefinedFirst_ReturnsFalse()
        {
            // Arrange
            var input =
@"enum UserType {
    User,
    User
}";
            var enumNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumDefinition());

            this.SetupMember(enumNode.enumMember()[0], "User");
            this.SetupMember(enumNode.enumMember()[1], "User");

            // Act
            var isAlreadyDefined = this.Binder.IsEnumMemberAlreadyDefined(
                "User", enumNode.enumMember()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        private void SetupMember(EnumMemberContext memberNode, string memberName)
        {
            var member = new EnumMemberBuilder()
                .SetNode(memberNode)
                .SetName(memberName)
                .Build();

            this.BinderProvider.GetBinder(memberNode).Returns(this.enumMemberBinder);
            this.enumMemberBinder.Bind<EnumMember>(memberNode).Returns(member);
        }
    }
}