namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Xunit;

    public class AllTypesTests : DocumentTests
    {
        [Fact]
        public void ContainsStruct_IncludesStruct()
        {
            // Arrange
            var input =
@"struct User {}";
            var document = this.CreateDocumentFromInput(input);

            var @struct = this.SetupMember(
                document.Node.definitions().structDefinition()[0], "User", document);

            // Act
            var allTypes = document.AllTypes;

            // Assert
            Assert.Collection(
                allTypes,
                member => Assert.Same(@struct, member));
        }

        [Fact]
        public void ContainsEnum_IncludesEnum()
        {
            // Arrange
            var input =
@"enum UserType {}";
            var document = this.CreateDocumentFromInput(input);

            var @enum = this.SetupMember(
                document.Node.definitions().enumDefinition()[0], "UserType", document);

            // Act
            var allTypes = document.AllTypes;

            // Assert
            Assert.Collection(
                allTypes,
                member => Assert.Same(@enum, member));
        }

        [Fact]
        public void ContainsMultipleDefinitions_ReturnsItemsInSourceOrder()
        {
            // Arrange
            var input =
@"enum UserType {}
struct User {}
enum PermissionType {}
struct Permission {}";
            var document = this.CreateDocumentFromInput(input);

            var userType = this.SetupMember(
                document.Node.definitions().enumDefinition()[0], "UserType", document);
            var permissionType = this.SetupMember(
                document.Node.definitions().enumDefinition()[1], "PermissionType", document);
            var user = this.SetupMember(
                document.Node.definitions().structDefinition()[0], "User", document);
            var permission = this.SetupMember(
                document.Node.definitions().structDefinition()[1], "Permission", document);

            // Act
            var allTypes = document.AllTypes;

            // Assert
            Assert.Collection(
                allTypes,
                member => Assert.Same(userType, member),
                member => Assert.Same(user, member),
                member => Assert.Same(permissionType, member),
                member => Assert.Same(permission, member));
        }
    }
}