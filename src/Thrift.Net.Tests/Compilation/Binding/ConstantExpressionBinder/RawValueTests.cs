namespace Thrift.Net.Tests.Compilation.Binding.ConstantExpressionBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    using ConstantExpressionBinder = Thrift.Net.Compilation.Binding.ConstantExpressionBinder;

    public class RawValueTests
    {
        [Fact]
        public void RawValueProvided_SetsRawValue()
        {
            // Arrange
            var binder = new ConstantExpressionBinder();
            var node = ParserInput.FromString("100")
                .ParseInput(parser => parser.constExpression());
            var parent = Substitute.For<IConstant>();

            // Act
            var value = binder.Bind<IConstantExpression>(node, parent);

            // Assert
            Assert.Equal("100", value.RawValue);
        }

        [Fact]
        public void RawValueNotProvided_Null()
        {
            // Arrange
            var binder = new ConstantExpressionBinder();
            var node = ParserInput.FromString(string.Empty)
                .ParseInput(parser => parser.constExpression());
            var parent = Substitute.For<IConstant>();

            // Act
            var value = binder.Bind<IConstantExpression>(node, parent);

            // Assert
            Assert.Null(value.RawValue);
        }
    }
}