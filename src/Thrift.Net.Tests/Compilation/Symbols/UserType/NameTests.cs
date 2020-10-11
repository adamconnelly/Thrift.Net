namespace Thrift.Net.Tests.Compilation.Symbols.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class NameTests
    {
        [Fact]
        public void ReturnsDefinitionName()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            definition.Name.Returns("UserType");
            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

            // Act
            var name = type.Name;

            // Assert
            Assert.Equal("UserType", name);
        }
    }
}