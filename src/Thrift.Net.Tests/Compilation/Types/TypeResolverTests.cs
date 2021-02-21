namespace Thrift.Net.Tests.Compilation.Types
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Types;
    using Xunit;

    public class TypeResolverTests
    {
        [Theory]
        [InlineData("bool")]
        [InlineData("byte")]
        [InlineData("i8")]
        [InlineData("i16")]
        [InlineData("i32")]
        [InlineData("i64")]
        [InlineData("double")]
        [InlineData("string")]
        [InlineData("binary")]
        [InlineData("slist")]
        public void CanResolveAllBaseTypes(string baseType)
        {
            // Arrange
            var typeResolver = new TypeResolver();
            var parent = Substitute.For<ISymbol>();
            Thrift.Net.Compilation.Types.BaseType.TryResolve(baseType, out var expectedType);

            // Act
            var result = typeResolver.Resolve(baseType, parent);

            // Assert
            Assert.True(result.IsResolved);
            Assert.True(result.Type.IsBaseType);
            Assert.Same(expectedType, result.Type);
        }

        [Fact]
        public void TypeCannotBeResolved_ReturnsUnresolvedType()
        {
            // Arrange
            var typeResolver = new TypeResolver();
            var parent = Substitute.For<ISymbol>();
            parent.ResolveType(default).ReturnsForAnyArgs((INamedTypeSymbol)null);

            // Act
            var result = typeResolver.Resolve("UnresolvedType", parent);

            // Assert
            Assert.False(result.IsResolved);
        }

        [Fact]
        public void TypeCanBeResolvedViaParent_ReturnsUserType()
        {
            // Arrange
            var typeResolver = new TypeResolver();
            var type = Substitute.For<INamedTypeSymbol>();

            var parent = Substitute.For<ISymbol>();
            parent.ResolveType("User").ReturnsForAnyArgs(type);

            // Act
            var result = typeResolver.Resolve("User", parent);

            // Assert
            var userType = Assert.IsType<Thrift.Net.Compilation.Types.UserType>(result.Type);
            Assert.Same(type, userType.Definition);
        }
    }
}