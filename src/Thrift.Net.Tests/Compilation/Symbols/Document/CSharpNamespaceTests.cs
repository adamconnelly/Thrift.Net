namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    public class CSharpNamespaceTests : DocumentTests
    {
        [Fact]
        public void CSharpNamespace_AllScopeProvided_ReturnsNamespace()
        {
            // Arrange
            var document = this.CreateDocumentFromInput("namespace * Thrift.Net.Tests");

            this.SetupNamespace(
                document.Node.header().namespaceStatement()[0],
                "*",
                "Thrift.Net.Tests",
                document);

            // Act
            var csharpNamespace = document.CSharpNamespace;

            // Assert
            Assert.Equal("Thrift.Net.Tests", csharpNamespace);
        }

        [Fact]
        public void CSharpNamespace_NoNamespacesProvided_ReturnsNull()
        {
            // Arrange
            var document = this.CreateDocumentFromInput("enum UserType {}");

            // Act
            var csharpNamespace = document.CSharpNamespace;

            // Assert
            Assert.Null(csharpNamespace);
        }

        [Theory]
        [InlineData("csharp")]
        [InlineData("netcore")]
        [InlineData("netstd")]
        public void CSharpNamespace_CSharpScopesTakePrecedenceOverAllScope(
            string csharpScope)
        {
            // Arrange
            var input =
@$"namespace {csharpScope} CSharpNamespace
namespace {Namespace.AllNamespacesScope} AllNamespace";
            var document = this.CreateDocumentFromInput(input);

            this.SetupNamespace(
                document.Node.header().namespaceStatement()[0],
                "csharp",
                "CSharpNamespace",
                document);

            this.SetupNamespace(
                document.Node.header().namespaceStatement()[1],
                Namespace.AllNamespacesScope,
                "AllNamespace",
                document);

            // Act
            var csharpNamespace = document.CSharpNamespace;

            // Assert
            Assert.Equal("CSharpNamespace", csharpNamespace);
        }

        [Fact]
        public void CSharpNamespace_MultipleValidSpecified_LastWins()
        {
            // Arrange
            var input =
@"namespace netstd NetStd
namespace csharp CSharp";
            var document = this.CreateDocumentFromInput(input);

            this.SetupNamespace(
                document.Node.header().namespaceStatement()[0],
                "netstd",
                "NetStd",
                document);

            this.SetupNamespace(
                document.Node.header().namespaceStatement()[1],
                "csharp",
                "CSharp",
                document);

            // Act
            var csharpNamespace = document.CSharpNamespace;

            // Assert
            Assert.Equal("CSharp", csharpNamespace);
        }
    }
}