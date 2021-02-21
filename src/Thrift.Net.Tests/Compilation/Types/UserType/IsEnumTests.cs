namespace Thrift.Net.Tests.Compilation.Types.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    using UserType = Thrift.Net.Compilation.Types.UserType;

    public class IsEnumTests
    {
        [Fact]
        public void DefinitionIsAnEnum_ReturnsTrue()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            var type = new UserType(definition);

            // Act
            var isEnum = type.IsEnum;

            // Assert
            Assert.True(isEnum);
        }

        [Fact]
        public void DefinitionIsNotAnEnum_ReturnsFalse()
        {
            // Arrange
            var definition = Substitute.For<IStruct>();
            var type = new UserType(definition);

            // Act
            var isEnum = type.IsEnum;

            // Assert
            Assert.False(isEnum);
        }
    }
}