namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Extensions;
    using Xunit;
    using ThriftCompiler = Thrift.Net.Compilation.ThriftCompiler;

    public class ConstantParsingTests
    {
        private readonly ThriftCompiler compiler = new ThriftCompiler();

        [Fact]
        public void Compile_DocumentContainsIntConstant_ParsesConstantName()
        {
            // Arrange
            var input = "const i32 MaxPageSize = 100";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Constants,
                item => Assert.Equal("MaxPageSize", item.Name));
        }

        [Fact]
        public void Compile_DocumentContainsIntConstant_ParsesConstantType()
        {
            // Arrange
            var input = "const i32 MaxPageSize = 100";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Constants,
                item => Assert.Equal(BaseType.I32Name, item.Type.Name));
        }

        [Fact]
        public void Compile_DocumentContainsIntConstant_ParsesExpressionType()
        {
            // Arrange
            var input = "const i32 MaxPageSize = 100";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Constants,
                item => Assert.Equal(BaseType.I8Name, item.Expression.Type.Name));
        }

        [Fact]
        public void Compile_DocumentContainsIntConstant_ParsesExpressionValue()
        {
            // Arrange
            var input = "const i32 MaxPageSize = 100";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Constants,
                item => Assert.Equal("100", item.Expression.RawValue));
        }

        [Theory]
        [InlineData("true", "true")]
        [InlineData("false", "false")]
        [InlineData("1", "true")]
        [InlineData("0", "false")]
        public void Compile_DocumentContainsBoolConstant_ParsesExpressionValue(string thriftValue, string csharpValue)
        {
            // Arrange
            var input = $"const bool BoolField = {thriftValue}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Constants,
                item => Assert.Equal(csharpValue, item.CSharpValue));
        }
    }
}