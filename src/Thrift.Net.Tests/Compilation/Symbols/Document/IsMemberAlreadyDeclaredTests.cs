namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;
    using static Thrift.Net.Antlr.ThriftParser;

    public class IsMemberAlreadyDeclaredTests : DocumentTests
    {
        [Fact]
        public void NoOtherMembersDeclared_ReturnsFalse()
        {
            // Arrange
            var input = "enum UserType {}";
            var document = this.CreateDocumentFromInput(input);

            this.SetupMember(document.Node.definitions().enumDefinition()[0], "UserType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(
                "UserType", document.Node.definitions().enumDefinition()[0]);

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
            this.SetupMember(document.Node.definitions().enumDefinition()[1], "UserType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(
                "UserType", document.Node.definitions().enumDefinition()[1]);

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

            this.SetupMember(document.Node.definitions().enumDefinition()[0], "UserType", document);
            this.SetupMember(document.Node.definitions().enumDefinition()[1], "UserType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(
                "UserType", document.Node.definitions().enumDefinition()[0]);

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
            this.SetupMember(document.Node.definitions().enumDefinition()[1], "PermissionType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(
                "PermissionType", document.Node.definitions().enumDefinition()[1]);

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
            this.SetupMember(document.Node.definitions().structDefinition()[0], "UserType", document);

            // Act
            var isAlreadyDefined = document.IsMemberNameAlreadyDeclared(
                "UserType", document.Node.definitions().structDefinition()[0]);

            // Assert
            Assert.True(isAlreadyDefined);
        }
    }
}