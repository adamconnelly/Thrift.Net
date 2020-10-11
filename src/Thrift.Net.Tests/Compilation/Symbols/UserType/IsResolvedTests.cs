namespace Thrift.Net.Tests.Compilation.Symbols.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class IsResolvedTests
    {
        [Fact]
        public void DefinitionIsResolvedType_ReturnsFalse()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

            // Act
            var isResolved = type.IsResolved;

            // Assert
            Assert.True(isResolved);
        }

        [Fact]
        public void DefinitionIsUnresolvedType_ReturnsFalse()
        {
            // Arrange
            var definition = Substitute.For<IUnresolvedType>();
            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

            // Act
            var isResolved = type.IsResolved;

            // Assert
            Assert.False(isResolved);
        }
    }
}