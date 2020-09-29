namespace Thrift.Net.Tests.Compilation.Binding.EnumMemberBinder
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class GetEnumValueTests : EnumMemberBinderTests
    {
        private readonly Enum parent;

        public GetEnumValueTests()
        {
            this.parent = new EnumBuilder().Build();
        }

        [Fact]
        public void NodeIsOnlyMember_ReturnsZero()
        {
            // Arrange
            var input =
@"enum UserType {
    User
}";
            var enumNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumDefinition());

            // Act
            var member = this.Binder.Bind<EnumMember>(enumNode.enumMember()[0], this.parent);

            // Assert
            Assert.Equal(0, member.Value);
        }

        [Fact]
        public void HasPreviousSibling_ReturnsNextValue()
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

            // Act
            var member = this.Binder.Bind<EnumMember>(enumNode.enumMember()[1], this.parent);

            // Assert
            Assert.Equal(1, member.Value);
        }

        [Fact]
        public void PreviousSiblingHasCustomValue_ReturnsNextValue()
        {
            // Arrange
            var input =
@"enum UserType {
    User = 5,
    Administrator
}";
            var enumNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumDefinition());

            // Act
            var member = this.Binder.Bind<EnumMember>(enumNode.enumMember()[1], this.parent);

            // Assert
            Assert.Equal(6, member.Value);
        }

        [Fact]
        public void PreviousSiblingValueIsInvalid_IgnoresPreviousSibling()
        {
            // Arrange
            var input =
@"enum UserType {
    User = 2,
    Administrator = -1,
    Guest
}";
            var enumNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumDefinition());

            // Act
            var member = this.Binder.Bind<EnumMember>(enumNode.enumMember()[2], this.parent);

            // Assert
            Assert.Equal(3, member.Value);
        }
    }
}