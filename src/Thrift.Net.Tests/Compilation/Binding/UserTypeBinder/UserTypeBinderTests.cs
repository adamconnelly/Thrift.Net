namespace Thrift.Net.Tests.Compilation.Binding
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class UserTypeBinderTests
    {
        [Fact]
        public void TypeCanBeResolvedFromParent_ResolvesType()
        {
            // Arrange
            var binder = new UserTypeBinder();
            var resolvedType = Substitute.For<INamedTypeSymbol>();
            var field = Substitute.For<IField>();
            field.ResolveType("UserType").Returns(resolvedType);

            var node = ParserInput
                .FromString("UserType")
                .ParseInput(parser => parser.userType());

            // Act
            var result = binder.Bind<UserType>(node, field);

            // Assert
            Assert.Same(resolvedType, result.Definition);
        }

        [Fact]
        public void TypeCannotBeResolvedFromParent_CreatesUnresolvedType()
        {
            // Arrange
            var binder = new UserTypeBinder();
            var field = Substitute.For<IField>();
            field.ResolveType(default).ReturnsForAnyArgs((INamedTypeSymbol)null);

            var node = ParserInput
                .FromString("UserType")
                .ParseInput(parser => parser.userType());

            // Act
            var result = binder.Bind<UserType>(node, field);

            // Assert
            Assert.False(result.IsResolved);
        }
    }
}