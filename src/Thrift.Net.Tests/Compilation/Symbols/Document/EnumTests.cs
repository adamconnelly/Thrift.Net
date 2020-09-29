namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Xunit;

    public class EnumTests : DocumentTests
    {
        [Fact]
        public void DocumentHasNoEnums_Empty()
        {
            // Arrange
            var document = this.CreateDocumentFromInput(string.Empty);

            // Act
            var enums = document.Enums;

            // Assert
            Assert.Empty(enums);
        }

        [Fact]
        public void EnumsProvided_SetsEnums()
        {
            // Arrange
            var input =
@"enum UserType {}
enum PermissionType {}";
            var document = this.CreateDocumentFromInput(input);

            var userTypeEnum = this.SetupMember(
                document.Node.definitions().enumDefinition()[0],
                "UserType",
                document);
            var permissionTypeEnum = this.SetupMember(
                document.Node.definitions().enumDefinition()[1],
                "PermissionType",
                document);

            // Act
            var enums = document.Enums;

            // Assert
            Assert.Collection(
                enums,
                userType => Assert.Same(userTypeEnum, userType),
                permissionEnum => Assert.Same(permissionEnum, permissionEnum));
        }
    }
}