namespace Thrift.Net.Tests.Compilation.Symbols.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class IsStructTests
    {
        [Fact]
        public void DefinitionIsAStruct_ReturnsTrue()
        {
            // Arrange
            var definition = Substitute.For<IStruct>();
            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

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
            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

            // Act
            var isStruct = type.IsStruct;

            // Assert
            Assert.False(isStruct);
        }
    }
}