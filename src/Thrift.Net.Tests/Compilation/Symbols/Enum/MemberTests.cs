namespace Thrift.Net.Tests.Compilation.Symbols.Enum
{
    using Xunit;

    public class MemberTests : EnumTests
    {
        [Fact]
        public void MembersSupplied_UsesBinderToCreateMembers()
        {
            // Arrange
            var input =
@"enum UserType {
    User,
    Administrator
}";
            var @enum = this.CreateEnumFromInput(input);

            var userMember = this.SetupMember(@enum.Node.enumMember()[0], @enum, name: "User");
            var administratorMember = this.SetupMember(@enum.Node.enumMember()[1], @enum, name: "Administrator");

            // Act
            var members = @enum.Members;

            // Assert
            Assert.Collection(
                members,
                user => Assert.Same(userMember, user),
                administrator => Assert.Same(administratorMember, administrator));
        }
    }
}