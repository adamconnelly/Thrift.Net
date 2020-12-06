namespace Thrift.Net.Tests.Compilation.Symbols.Exception
{
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    public class OptionalFieldTests : ExceptionTests
    {
        [Fact]
        public void HasOptionalFields_ReturnsOptionalFields()
        {
            // Arrange
            var input =
@"exception User {
    1: i32 DefaultField
    2: required i32 RequiredField
    3: required i32 OptionalField
}";
            var exception = this.CreateExceptionFromInput(input);

            var defaultField = this.SetupField(exception.Node.field()[0], exception, requiredness: FieldRequiredness.Default);
            var requiredField = this.SetupField(exception.Node.field()[1], exception, requiredness: FieldRequiredness.Required);
            var optionalField = this.SetupField(exception.Node.field()[2], exception, requiredness: FieldRequiredness.Optional);

            // Act
            var fields = exception.OptionalFields;

            // Assert
            Assert.Collection(
                fields,
                @default => Assert.Same(defaultField, @default),
                optional => Assert.Same(optionalField, optional));
        }
    }
}