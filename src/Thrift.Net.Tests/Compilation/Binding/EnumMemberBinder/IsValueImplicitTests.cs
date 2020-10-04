namespace Thrift.Net.Tests.Compilation.Binding.EnumMemberBinder
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class IsValueImplicitTests : EnumMemberBinderTests
    {
        [Theory]
        [InlineData("User", true)]
        [InlineData("User = 1", false)]
        [InlineData("User 1", false)]
        [InlineData("User = 'abc'", false)]
        [InlineData("= 1", false)]
        [InlineData("User =", false)]
        public void IndicatesWhenValueIsImplicit(string input, bool isImplicit)
        {
            // Arrange
            var @enum = new EnumBuilder().Build();
            var memberNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.enumMember());

            // Act
            var member = this.Binder.Bind<EnumMember>(memberNode, @enum);

            // Assert
            Assert.Equal(isImplicit, member.IsValueImplicit);
        }
    }
}