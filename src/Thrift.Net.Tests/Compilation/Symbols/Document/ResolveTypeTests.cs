namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Xunit;

    public class ResolveTypeTests : DocumentTests
    {
        [Fact]
        public void HasEnumWithName_ReturnsResolvedType()
        {
            // Arrange
            var input = @"enum UserType {}";
            var document = this.CreateDocumentFromInput(input);

            this.SetupMember(
                document.Node.definitions().enumDefinition()[0], "UserType", document);

            // Act
            var resolvedType = document.ResolveType("UserType");

            // Assert
            Assert.True(resolvedType.IsResolved);
            Assert.Equal("UserType", resolvedType.CSharpTypeName);
        }

        [Fact]
        public void HasStructWithName_ReturnsResolvedType()
        {
            // Arrange
            var input = @"struct User {}";
            var document = this.CreateDocumentFromInput(input);

            this.SetupMember(
                document.Node.definitions().structDefinition()[0], "User", document);

            // Act
            var resolvedType = document.ResolveType("User");

            // Assert
            Assert.True(resolvedType.IsResolved);
            Assert.Equal("User", resolvedType.CSharpTypeName);
        }

        [Fact]
        public void HasNoMemberWithName_ReturnsNull()
        {
            // Arrange
            var input = @"enum UserType {}";
            var document = this.CreateDocumentFromInput(input);

            this.SetupMember(
                document.Node.definitions().enumDefinition()[0], "UserType", document);

            // Act
            var resolvedType = document.ResolveType("User");

            // Assert
            Assert.Null(resolvedType);
        }

        [Fact]
        public void DocumentHasNamespace_IncludesNamespaceInName()
        {
            // Arrange
            var input =
@"namespace csharp Thrift.Net.Tests
enum UserType {}";
            var document = this.CreateDocumentFromInput(input);

            this.SetupNamespace(
                document.Node.header().namespaceStatement()[0], "csharp", "Thrift.Net.Tests", document);
            this.SetupMember(
                document.Node.definitions().enumDefinition()[0], "UserType", document);

            // Act
            var resolvedType = document.ResolveType("UserType");

            // Assert
            Assert.Equal("Thrift.Net.Tests.UserType", resolvedType.CSharpTypeName);
        }
    }
}