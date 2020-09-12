namespace Thrift.Net.Tests.Compilation.Binding.StructBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Model;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class FieldTests : StructBinderTests
    {
        private readonly IBinder fieldBinder = Substitute.For<IBinder>();

        public FieldTests()
        {
            this.BinderProvider.GetBinder(default).ReturnsForAnyArgs(this.fieldBinder);
        }

        [Fact]
        public void Bind_FieldsSupplied_UsesBinderToCreateFields()
        {
            // Arrange
            var input =
@"struct User {
    1: i32 Id
    2: string Username
}";
            var structContext = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            var idField = new FieldDefinition(
                1, "1", FieldRequiredness.Default, FieldType.I32, "Id", false);
            this.fieldBinder.Bind<FieldDefinition>(structContext.field()[0])
                .Returns(idField);
            var usernameField = new FieldDefinition(
                2, "2", FieldRequiredness.Default, FieldType.I32, "Username", false);
            this.fieldBinder.Bind<FieldDefinition>(structContext.field()[1])
                .Returns(usernameField);

            // Act
            var structDefinition = this.Binder.Bind<StructDefinition>(structContext);

            // Assert
            Assert.Collection(
                structDefinition.Fields,
                id => Assert.Same(idField, id),
                username => Assert.Same(usernameField, username));
        }
    }
}