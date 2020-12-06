namespace Thrift.Net.Tests.Compilation.Symbols.Exception
{
    using Xunit;

    public class FieldTests : ExceptionTests
    {
        [Fact]
        public void Bind_FieldsSupplied_UsesBinderToCreateFields()
        {
            // Arrange
            var input =
@"exception NotFoundException {
    1: i32 Id
    2: string Username
}";
            var exception = this.CreateExceptionFromInput(input);

            var idField = this.SetupField(exception.Node.field()[0], exception);
            var usernameField = this.SetupField(exception.Node.field()[1], exception);

            // Act
            var fields = exception.Fields;

            // Assert
            Assert.Collection(
                fields,
                id => Assert.Same(idField, id),
                username => Assert.Same(usernameField, username));
        }
    }
}