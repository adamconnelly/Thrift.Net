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
            Assert.Equal("UserType", resolvedType.Name);
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
            Assert.Equal("User", resolvedType.Name);
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
    }
}