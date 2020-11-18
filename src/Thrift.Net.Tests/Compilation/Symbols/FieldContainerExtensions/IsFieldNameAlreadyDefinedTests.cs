namespace Thrift.Net.Tests.Compilation.Symbols.FieldContainerExtensions
{
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    public class IsFieldNameAlreadyDefinedTests : FieldContainerExtensionsTests
    {
        [Fact]
        public void SingleField_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    string Username
}";
            var @struct = this.CreateStructFromInput(input);

            this.SetupField(@struct.Node.field()[0], @struct, name: "Username");

            // Act
            var isAlreadyDefined = @struct.IsFieldNameAlreadyDefined(
                "Username", @struct.Node.field()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void DuplicateField_ReturnsTrue()
        {
            // Arrange
            var input =
@"struct User {
    string Username
    string Username
}";
            var @struct = this.CreateStructFromInput(input);

            this.SetupField(@struct.Node.field()[0], @struct, name: "Username");
            this.SetupField(@struct.Node.field()[1], @struct, name: "Username");

            // Act
            var isAlreadyDefined = @struct.IsFieldNameAlreadyDefined(
                "Username", @struct.Node.field()[1]);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void NoDuplicates_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
    string Username
}";
            var @struct = this.CreateStructFromInput(input);

            this.SetupField(@struct.Node.field()[0], @struct, name: "Id");
            this.SetupField(@struct.Node.field()[1], @struct, name: "Username");

            // Act
            var isAlreadyDefined = @struct.IsFieldNameAlreadyDefined(
                "Username", @struct.Node.field()[1]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void FirstFieldChosen_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    string Username
    string Username
}";
            var @struct = this.CreateStructFromInput(input);

            this.SetupField(@struct.Node.field()[0], @struct, name: "Username");
            this.SetupField(@struct.Node.field()[1], @struct, name: "Username");

            // Act
            var isAlreadyDefined = @struct.IsFieldNameAlreadyDefined(
                "Username", @struct.Node.field()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }
    }
}