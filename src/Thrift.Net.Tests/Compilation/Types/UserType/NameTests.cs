namespace Thrift.Net.Tests.Compilation.Types.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    using UserType = Thrift.Net.Compilation.Types.UserType;

    public class NameTests
    {
        [Fact]
        public void ReturnsDefinitionName()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            definition.Name.Returns("UserType");
            var type = new UserType(definition);

            // Act
            var name = type.Name;

            // Assert
            Assert.Equal("UserType", name);
        }
    }
}