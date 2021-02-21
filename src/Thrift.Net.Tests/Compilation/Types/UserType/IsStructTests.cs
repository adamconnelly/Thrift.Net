namespace Thrift.Net.Tests.Compilation.Types.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    using UserType = Thrift.Net.Compilation.Types.UserType;

    public class IsStructTests
    {
        [Fact]
        public void DefinitionIsAStruct_ReturnsTrue()
        {
            // Arrange
            var definition = Substitute.For<IStruct>();
            var type = new UserType(definition);

            // Act
            var isStruct = type.IsStruct;

            // Assert
            Assert.True(isStruct);
        }

        [Fact]
        public void DefinitionIsNotAStruct_ReturnsFalse()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            var type = new UserType(definition);

            // Act
            var isStruct = type.IsStruct;

            // Assert
            Assert.False(isStruct);
        }
    }
}