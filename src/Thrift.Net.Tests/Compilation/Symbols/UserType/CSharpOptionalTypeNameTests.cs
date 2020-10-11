namespace Thrift.Net.Tests.Compilation.Symbols.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class CSharpOptionalTypeNameTests
    {
        [Fact]
        public void IsEnum_ReturnsNullableTypeName()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            definition.Name.Returns("UserType");
            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

            // Act
            var name = type.CSharpOptionalTypeName;

            // Assert
            Assert.Equal("UserType?", name);
        }

        [Fact]
        public void DocumentHasNamespace_IncludesNamespaceInName()
        {
            // Arrange
            var definition = Substitute.For<IEnum>();
            definition.Name.Returns("UserType");
            definition.Document.CSharpNamespace.Returns("Thrift.Net.Tests");

            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

            // Act
            var name = type.CSharpOptionalTypeName;

            // Assert
            Assert.Equal("Thrift.Net.Tests.UserType?", name);
        }

        [Fact]
        public void IsStruct_ReturnsTypeName()
        {
            // Arrange
            var definition = Substitute.For<IStruct>();
            definition.Name.Returns("User");
            var type = new UserTypeBuilder()
                .SetDefinition(definition)
                .Build();

            // Act
            var name = type.CSharpOptionalTypeName;

            // Assert
            Assert.Equal("User", name);
        }
    }
}