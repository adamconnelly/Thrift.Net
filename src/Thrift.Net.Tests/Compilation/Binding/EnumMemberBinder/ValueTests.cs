namespace Thrift.Net.Tests.Compilation.Binding.EnumMemberBinder
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class ValueTests : EnumMemberBinderTests
    {
        [Theory]
        [InlineData("User = 1", 1)]
        [InlineData("User =", null)]
        [InlineData("User = abc", null)]
        [InlineData("User = 'abc'", null)]
        [InlineData("User = -1", null)]
        [InlineData("User = 0", 0)]
        public void SetsValueCorrectly(string input, int? expected)
        {
            // Arrange
            var @enum = new EnumBuilder().Build();
            var memberNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumMember());

            // Act
            var member = this.Binder.Bind<EnumMember>(memberNode, @enum);

            // Assert
            Assert.Equal(expected, member.Value);
        }

        [Theory]
        [InlineData("User = 1", "1")]
        [InlineData("User =", null)]
        [InlineData("User = 'abc'", "'abc'")]
        [InlineData("User = -1", "-1")]
        public void SetsRawValueCorrectly(string input, string expected)
        {
            // Arrange
            var @enum = new EnumBuilder().Build();
            var memberNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumMember());

            // Act
            var member = this.Binder.Bind<EnumMember>(memberNode, @enum);

            // Assert
            Assert.Equal(expected, member.RawValue);
        }

        [Theory]
        [InlineData("User", InvalidEnumValueReason.None)]
        [InlineData("User = 1", InvalidEnumValueReason.None)]
        [InlineData("User =", InvalidEnumValueReason.Missing)]
        [InlineData("User = 'abc'", InvalidEnumValueReason.NotAnInteger)]
        [InlineData("User = -1", InvalidEnumValueReason.Negative)]
        public void SetsInvalidValueReasonCorrectly(string input, InvalidEnumValueReason expected)
        {
            // Arrange
            var @enum = new EnumBuilder().Build();
            var memberNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumMember());

            // Act
            var member = this.Binder.Bind<EnumMember>(memberNode, @enum);

            // Assert
            Assert.Equal(expected, member.InvalidValueReason);
        }
    }
}