namespace Thrift.Net.Tests.Compilation.Symbols.Union
{
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    public class OptionalFieldTests : UnionTests
    {
        [Fact]
        public void HasOptionalFields_ReturnsOptionalFields()
        {
            // Arrange
            var input =
@"union User {
    1: i32 DefaultField
    2: required i32 RequiredField
    3: required i32 OptionalField
}";
            var union = this.CreateUnionFromInput(input);

            var defaultField = this.SetupField(union.Node.field()[0], union, requiredness: FieldRequiredness.Default);
            var requiredField = this.SetupField(union.Node.field()[1], union, requiredness: FieldRequiredness.Required);
            var optionalField = this.SetupField(union.Node.field()[2], union, requiredness: FieldRequiredness.Optional);

            // Act
            var fields = union.OptionalFields;

            // Assert
            Assert.Collection(
                fields,
                @default => Assert.Same(defaultField, @default),
                optional => Assert.Same(optionalField, optional));
        }
    }
}