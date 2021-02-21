namespace Thrift.Net.Tests.Compilation.Types.UserType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Types;
    using Xunit;

    using UserType = Thrift.Net.Compilation.Types.UserType;

    public class IsResolvedTests
    {
        // [Fact]
        // public void DefinitionIsResolvedType_ReturnsFalse()
        // {
        //     // Arrange
        //     var definition = Substitute.For<IEnum>();
        //     var type = new UserType(definition);

        //     // Act
        //     var isResolved = type.IsResolved;

        //     // Assert
        //     Assert.True(isResolved);
        // }

        // [Fact]
        // public void DefinitionIsUnresolvedType_ReturnsFalse()
        // {
        //     // Arrange
        //     var definition = Substitute.For<IUnresolvedType>();
        //     var type = new UserType(definition);

        //     // Act
        //     var isResolved = type.IsResolved;

        //     // Assert
        //     Assert.False(isResolved);
        // }
    }
}