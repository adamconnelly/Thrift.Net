namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Xunit;

    public class NamespaceTests : DocumentTests
    {
        [Fact]
        public void DocumentHasNoNamespaces_Empty()
        {
            // Arrange
            var document = this.CreateDocumentFromInput(string.Empty);

            // Act
            var namespaces = document.Namespaces;

            // Assert
            Assert.Empty(namespaces);
        }

        [Fact]
        public void NamespacesProvided_CreatesNamespaces()
        {
            // Arrange
            var input =
@"namespace * All
namespace csharp Thrift.Net.Examples";
            var document = this.CreateDocumentFromInput(input);

            var allNamespace = this.SetupNamespace(
                document.Node.header().namespaceStatement()[0],
                "*",
                "All",
                document);

            var csharpNamespace = this.SetupNamespace(
                document.Node.header().namespaceStatement()[1],
                "csharp",
                "Thrift.Net.Examples",
                document);

            // Act
            var namespaces = document.Namespaces;

            // Assert
            Assert.Collection(
                namespaces,
                all => Assert.Same(allNamespace, all),
                csharp => Assert.Same(csharpNamespace, csharp));
        }
    }
}