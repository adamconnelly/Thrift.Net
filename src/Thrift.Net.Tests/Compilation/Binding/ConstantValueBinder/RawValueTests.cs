namespace Thrift.Net.Tests.Compilation.Binding.ConstantValueBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    using ConstantValueBinder = Thrift.Net.Compilation.Binding.ConstantValueBinder;

    public class RawValueTests
    {
        [Fact]
        public void RawValueProvided_SetsRawValue()
        {
            // Arrange
            var binder = new ConstantValueBinder();
            var node = ParserInput.FromString("100")
                .ParseInput(parser => parser.constExpression());
            var parent = Substitute.For<IConstant>();

            // Act
            var value = binder.Bind<IConstantValue>(node, parent);

            // Assert
            Assert.Equal("100", value.RawValue);
        }

        [Fact]
        public void RawValueNotProvided_Null()
        {
            // Arrange
            var binder = new ConstantValueBinder();
            var node = ParserInput.FromString(string.Empty)
                .ParseInput(parser => parser.constExpression());
            var parent = Substitute.For<IConstant>();

            // Act
            var value = binder.Bind<IConstantValue>(node, parent);

            // Assert
            Assert.Null(value.RawValue);
        }
    }
}