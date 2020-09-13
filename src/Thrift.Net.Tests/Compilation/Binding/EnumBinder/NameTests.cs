namespace Thrift.Net.Tests.Compilation.Binding.EnumBinder
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class NameTests : EnumBinderTests
    {
        [Theory]
        [InlineData("enum User {}", "User")]
        [InlineData("enum {}", null)]
        public void SetsNameCorrectly(string input, string name)
        {
            // Arrange
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumDefinition());

            // Act
            var symbol = this.Binder.Bind<Enum>(structNode);

            // Assert
            Assert.Equal(name, symbol.Name);
        }
    }
}