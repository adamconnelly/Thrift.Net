namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Thrift.Net.Tests.Extensions;
    using Xunit;

    public class NamespaceParsingTests
    {
        [Theory]
        [InlineData("namespace * Thrift.Net.Examples", "Thrift.Net.Examples")]
        [InlineData("namespace delphi Thrift.Net.Examples", null)]
        [InlineData("namespace csharp Thrift.Net.Examples", "Thrift.Net.Examples")]
        [InlineData("namespace netcore Thrift.Net.Examples", "Thrift.Net.Examples")]
        public void Compile_SetsNamespace(string input, string expected)
        {
            // Arrange
            var compiler = new ThriftCompiler();

            // Act
            var result = compiler.Compile(input.ToStream());

            // Assert
            Assert.Equal(expected, result.Document.Namespace);
        }

        [Fact]
        public void Compile_MultipleNamespacesSpecified_IgnoresNonCSharpNamespaces()
        {
            // Arrange
            var compiler = new ThriftCompiler();
            string input =
@"namespace csharp Thrift.Net.Examples
namespace delphi SomeOtherNamespace";

            // Act
            var result = compiler.Compile(input.ToStream());

            // Assert
            Assert.Equal("Thrift.Net.Examples", result.Document.Namespace);
        }
    }
}