namespace Thrift.Net.Tests.Compilation.Symbols.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class IsEnumTests
    {
        [Fact]
        public void DefinitionIsAnEnum_ReturnsTrue()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

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
            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

            // Act
            var isEnum = type.IsEnum;

            // Assert
            Assert.False(isEnum);
        }
    }
}