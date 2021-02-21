namespace Thrift.Net.Tests.Compilation.Types.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    using UserType = Thrift.Net.Compilation.Types.UserType;

    public class CSharpRequiredTypeNameTests
    {
        [Fact]
        public void IsEnum_ReturnsTypeName()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            definition.Name.Returns("UserType");
            var type = new UserType(definition);

            // Act
            var name = type.CSharpRequiredTypeName;

            // Assert
            Assert.Equal("UserType", name);
        }

        [Fact]
        public void DocumentHasNamespace_IncludesNamespaceInName()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            definition.Name.Returns("UserType");
            definition.Document.CSharpNamespace.Returns("Thrift.Net.Tests");

            var type = new UserType(definition);

            // Act
            var name = type.CSharpRequiredTypeName;

            // Assert
            Assert.Equal("Thrift.Net.Tests.UserType", name);
        }

        [Fact]
        public void IsStruct_ReturnsTypeName()
        {
            // Arrange
            var definition = Substitute.For<IStruct>();
            definition.Name.Returns("User");
            var type = new UserType(definition);

            // Act
            var name = type.CSharpRequiredTypeName;

            // Assert
            Assert.Equal("User", name);
        }
    }
}