namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Xunit;

    public class IsNamespaceForScopeAlreadySpecified : DocumentTests
    {
        [Fact]
        public void SingleNamespace_ReturnsFalse()
        {
            var input = "namespace csharp Thrift.Net.Examples";
            var document = this.CreateDocumentFromInput(input);

            var @namespace = this.SetupNamespace(
                document.Node.header().namespaceStatement()[0],
                "csharp",
                "Thrift.Net.Examples",
                document);

            // Act
            var isAlreadyDefined = document.IsNamespaceForScopeAlreadyDeclared(
                @namespace);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void NamespaceScopeAlreadyDeclared_ReturnsTrue()
        {
            var input =
@"namespace csharp Thrift.Net.Examples.A
namespace csharp Thrift.Net.Examples.B";
            var document = this.CreateDocumentFromInput(input);

            this.SetupNamespace(
                document.Node.header().namespaceStatement()[0],
                "csharp",
                "Thrift.Net.Examples.A",
                document);

            var @namespace = this.SetupNamespace(
                document.Node.header().namespaceStatement()[1],
                "csharp",
                "Thrift.Net.Examples.B",
                document);

            // Act
            var isAlreadyDefined = document.IsNamespaceForScopeAlreadyDeclared(
                @namespace);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void FirstDeclarationOfScope_ReturnsFalse()
        {
            var input =
@"namespace csharp Thrift.Net.Examples.A
namespace csharp Thrift.Net.Examples.B";
            var document = this.CreateDocumentFromInput(input);

            var @namespace = this.SetupNamespace(
                document.Node.header().namespaceStatement()[0],
                "csharp",
                "Thrift.Net.Examples.A",
                document);

            this.SetupNamespace(
                document.Node.header().namespaceStatement()[1],
                "csharp",
                "Thrift.Net.Examples.B",
                document);

            // Act
            var isAlreadyDefined = document.IsNamespaceForScopeAlreadyDeclared(
                @namespace);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void ScopeNotDuplicated_ReturnsFalse()
        {
            var input =
@"namespace csharp Thrift.Net.Examples.A
namespace netstd Thrift.Net.Examples.B";
            var document = this.CreateDocumentFromInput(input);

            this.SetupNamespace(
                document.Node.header().namespaceStatement()[0],
                "csharp",
                "Thrift.Net.Examples.A",
                document);

            var @namespace = this.SetupNamespace(
                document.Node.header().namespaceStatement()[1],
                "netstd",
                "Thrift.Net.Examples.B",
                document);

            // Act
            var isAlreadyDefined = document.IsNamespaceForScopeAlreadyDeclared(
                @namespace);

            // Assert
            Assert.False(isAlreadyDefined);
        }
    }
}