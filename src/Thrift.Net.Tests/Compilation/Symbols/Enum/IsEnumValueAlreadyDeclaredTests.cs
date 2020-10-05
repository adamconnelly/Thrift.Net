namespace Thrift.Net.Tests.Compilation.Symbols.Enum
{
    using Xunit;

    public class IsEnumValueAlreadyDeclaredTests : EnumTests
    {
        [Fact]
        public void SingleMember_ReturnsFalse()
        {
            // Arrange
            var input =
@"enum UserType {
    User = 0
}";
            var @enum = this.CreateEnumFromInput(input);

            var member = this.SetupMember(@enum.Node.enumMember()[0], @enum, value: 0);

            // Act
            var isAlreadyDefined = @enum.IsEnumValueAlreadyDeclared(member);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void ValueAlreadyUsed_ReturnsTrue()
        {
            // Arrange
            var input =
@"enum UserType {
    User = 1,
    Administrator = 1
}";
            var @enum = this.CreateEnumFromInput(input);

            this.SetupMember(@enum.Node.enumMember()[0], @enum, value: 1);
            var duplicate = this.SetupMember(@enum.Node.enumMember()[1], @enum, value: 1);

            // Act
            var isAlreadyDefined = @enum.IsEnumValueAlreadyDeclared(duplicate);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void MemberDefinedFirst_ReturnsFalse()
        {
            // Arrange
            var input =
@"enum UserType {
    User = 1,
    Administrator = 1
}";
            var @enum = this.CreateEnumFromInput(input);

            var original = this.SetupMember(@enum.Node.enumMember()[0], @enum, value: 1);
            this.SetupMember(@enum.Node.enumMember()[1], @enum, value: 1);

            // Act
            var isAlreadyDefined = @enum.IsEnumValueAlreadyDeclared(original);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void ValueIsInvalid_ReturnsFalse()
        {
            // Arrange
            var input =
@"enum UserType {
    User = 'abc',
    Administrator = 'abc'
}";
            var @enum = this.CreateEnumFromInput(input);

            this.SetupMember(@enum.Node.enumMember()[0], @enum, value: null);
            var administrator = this.SetupMember(@enum.Node.enumMember()[1], @enum, value: null);

            // Act
            var isAlreadyDefined = @enum.IsEnumValueAlreadyDeclared(administrator);

            // Assert
            Assert.False(isAlreadyDefined);
        }
    }
}