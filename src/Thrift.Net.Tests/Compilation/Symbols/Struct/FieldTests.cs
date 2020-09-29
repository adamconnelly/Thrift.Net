namespace Thrift.Net.Tests.Compilation.Symbols.Struct
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class FieldTests : StructTests
    {
        [Fact]
        public void Bind_FieldsSupplied_UsesBinderToCreateFields()
        {
            // Arrange
            var input =
@"struct User {
    1: i32 Id
    2: string Username
}";
            var @struct = this.CreateStructFromInput(input);

            var idField = this.SetupField(@struct.Node.field()[0], @struct);
            var usernameField = this.SetupField(@struct.Node.field()[1], @struct);

            // Act
            var fields = @struct.Fields;

            // Assert
            Assert.Collection(
                fields,
                id => Assert.Same(idField, id),
                username => Assert.Same(usernameField, username));
        }
    }
}