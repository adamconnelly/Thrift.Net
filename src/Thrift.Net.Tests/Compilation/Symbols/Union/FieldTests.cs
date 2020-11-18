namespace Thrift.Net.Tests.Compilation.Symbols.Union
{
    using Xunit;

    public class FieldTests : UnionTests
    {
        [Fact]
        public void Bind_FieldsSupplied_UsesBinderToCreateFields()
        {
            // Arrange
            var input =
@"union User {
    1: i32 Id
    2: string Username
}";
            var union = this.CreateUnionFromInput(input);

            var idField = this.SetupField(union.Node.field()[0], union);
            var usernameField = this.SetupField(union.Node.field()[1], union);

            // Act
            var fields = union.Fields;

            // Assert
            Assert.Collection(
                fields,
                id => Assert.Same(idField, id),
                username => Assert.Same(usernameField, username));
        }
    }
}