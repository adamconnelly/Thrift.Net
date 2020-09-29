namespace Thrift.Net.Tests.Compilation.Binding.EnumMemberBinder
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class NameTests : EnumMemberBinderTests
    {
        [Theory]
        [InlineData("User", "User")]
        [InlineData("= 1", null)]
        public void SetsNameCorrectly(string input, string expectedResult)
        {
            // Arrange
            var @enum = new EnumBuilder().Build();
            var memberNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumMember());

            // Act
            var member = this.Binder.Bind<EnumMember>(memberNode, @enum);

            // Assert
            Assert.Equal(expectedResult, member.Name);
        }
    }
}