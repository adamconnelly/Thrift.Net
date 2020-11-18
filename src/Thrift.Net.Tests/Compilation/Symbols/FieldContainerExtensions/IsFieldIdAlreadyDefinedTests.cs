namespace Thrift.Net.Tests.Compilation.Symbols.FieldContainerExtensions
{
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    public class IsFieldIdAlreadyDefinedTests : FieldContainerExtensionsTests
    {
        [Fact]
        public void SingleField_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    0: string Username
}";
            var @struct = this.CreateStructFromInput(input);

            this.SetupField(@struct.Node.field()[0], @struct, 0);

            // Act
            var isAlreadyDefined = @struct.IsFieldIdAlreadyDefined(
                0, @struct.Node.field()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void DuplicateField_ReturnsTrue()
        {
            // Arrange
            var input =
@"struct User {
    1: i32 Id
    1: string Username
}";
            var @struct = this.CreateStructFromInput(input);

            this.SetupField(@struct.Node.field()[0], @struct, 1);
            this.SetupField(@struct.Node.field()[1], @struct, 1);

            // Act
            var isAlreadyDefined = @struct.IsFieldIdAlreadyDefined(
                1, @struct.Node.field()[1]);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void NoDuplicates_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    0: i32 Id
    1: string Username
}";
            var @struct = this.CreateStructFromInput(input);

            this.SetupField(@struct.Node.field()[0], @struct, 0);
            this.SetupField(@struct.Node.field()[1], @struct, 1);

            // Act
            var isAlreadyDefined = @struct.IsFieldIdAlreadyDefined(
                1, @struct.Node.field()[1]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void FirstFieldChosen_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    1: string Username
    1: string Username
}";
            var @struct = this.CreateStructFromInput(input);

            this.SetupField(@struct.Node.field()[0], @struct, 1);
            this.SetupField(@struct.Node.field()[1], @struct, 1);

            // Act
            var isAlreadyDefined = @struct.IsFieldIdAlreadyDefined(
                1, @struct.Node.field()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }
    }
}