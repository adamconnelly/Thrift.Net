namespace Thrift.Net.Tests.Compilation.Symbols.Enum
{
    using Xunit;

    public class IsEnumMemberAlreadyDefinedTests : EnumTests
    {
        [Fact]
        public void SingleMember_ReturnsFalse()
        {
            // Arrange
            var input =
@"enum UserType {
    User
}";
            var @enum = this.CreateEnumFromInput(input);

            this.SetupMember(@enum.Node.enumMember()[0], @enum, name: "User");

            // Act
            var isAlreadyDefined = @enum.IsEnumMemberAlreadyDeclared(
                "User", @enum.Node.enumMember()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void MemberAlreadyDefined_ReturnsTrue()
        {
            // Arrange
            var input =
@"enum UserType {
    User,
    User
}";
            var @enum = this.CreateEnumFromInput(input);

            this.SetupMember(@enum.Node.enumMember()[0], @enum, name: "User");
            this.SetupMember(@enum.Node.enumMember()[1], @enum, name: "User");

            // Act
            var isAlreadyDefined = @enum.IsEnumMemberAlreadyDeclared(
                "User", @enum.Node.enumMember()[1]);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void MemberDefinedFirst_ReturnsFalse()
        {
            // Arrange
            var input =
@"enum UserType {
    User,
    User
}";
            var @enum = this.CreateEnumFromInput(input);

            this.SetupMember(@enum.Node.enumMember()[0], @enum, name: "User");
            this.SetupMember(@enum.Node.enumMember()[1], @enum, name: "User");

            // Act
            var isAlreadyDefined = @enum.IsEnumMemberAlreadyDeclared(
                "User", @enum.Node.enumMember()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }
    }
}