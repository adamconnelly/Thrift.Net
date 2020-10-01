namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Xunit;

    public class IsMemberAlreadyDeclaredTests : DocumentTests
    {
        [Fact]
        public void NoOtherMembersDeclared_ReturnsFalse()
        {
            // Arrange
            var input = "enum UserType {}";
            var document = this.CreateDocumentFromInput(input);

            var @enum = this.SetupMember(document.Node.definitions().enumDefinition()[0], "UserType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(@enum);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void HasDuplicateEnum_ReturnsTrue()
        {
            var input =
@"enum UserType {}
enum UserType {}";
            var document = this.CreateDocumentFromInput(input);

            this.SetupMember(document.Node.definitions().enumDefinition()[0], "UserType", document);
            var duplicate = this.SetupMember(document.Node.definitions().enumDefinition()[1], "UserType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(duplicate);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void MemberDeclaredFirst_ReturnsFalse()
        {
            var input =
@"enum UserType {}
enum UserType {}";
            var document = this.CreateDocumentFromInput(input);

            var original = this.SetupMember(document.Node.definitions().enumDefinition()[0], "UserType", document);
            this.SetupMember(document.Node.definitions().enumDefinition()[1], "UserType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(original);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void MemberNotADuplicate_ReturnsFalse()
        {
            var input =
@"enum UserType {}
enum PermissionType {}";
            var document = this.CreateDocumentFromInput(input);

            this.SetupMember(document.Node.definitions().enumDefinition()[0], "UserType", document);
            var permissionType = this.SetupMember(document.Node.definitions().enumDefinition()[1], "PermissionType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(permissionType);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void MultipleMemberTypesWithDuplicateNames_ReturnTrue()
        {
            var input =
@"enum UserType {}
struct UserType {}";
            var document = this.CreateDocumentFromInput(input);

            this.SetupMember(document.Node.definitions().enumDefinition()[0], "UserType", document);
            var @struct = this.SetupMember(document.Node.definitions().structDefinition()[0], "UserType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(@struct);

            // Assert
            Assert.True(isAlreadyDefined);
        }
    }
}