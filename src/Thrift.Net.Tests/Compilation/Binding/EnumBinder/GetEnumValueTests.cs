namespace Thrift.Net.Tests.Compilation.Binding.EnumBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;
    using static Thrift.Net.Antlr.ThriftParser;

    public class GetEnumValueTests : EnumBinderTests
    {
        private readonly IBinder enumMemberBinder = Substitute.For<IBinder>();

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
            var value = this.Binder.GetEnumValue(enumNode.enumMember()[0]);

            // Assert
            Assert.Equal(0, value);
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

            this.SetupMember(enumNode.enumMember()[0], 0);

            // Act
            var value = this.Binder.GetEnumValue(enumNode.enumMember()[1]);

            // Assert
            Assert.Equal(1, value);
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

            this.SetupMember(enumNode.enumMember()[0], 5);

            // Act
            var value = this.Binder.GetEnumValue(enumNode.enumMember()[1]);

            // Assert
            Assert.Equal(6, value);
        }

        [Fact]
        public void PreviousSiblingValueIsNull_IgnoresPreviousSibling()
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

            this.SetupMember(enumNode.enumMember()[0], 2);
            this.SetupMember(enumNode.enumMember()[1], null);

            // Act
            var value = this.Binder.GetEnumValue(enumNode.enumMember()[2]);

            // Assert
            Assert.Equal(3, value);
        }

        private void SetupMember(EnumMemberContext memberNode, int? value)
        {
            var member = new EnumMemberBuilder()
                .SetValue(value)
                .Build();

            this.BinderProvider.GetBinder(memberNode).Returns(this.enumMemberBinder);
            this.enumMemberBinder.Bind<EnumMember>(memberNode).Returns(member);
        }
    }
}