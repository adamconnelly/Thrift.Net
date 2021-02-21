namespace Thrift.Net.Tests.Compilation.Types.SetType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Compilation.Types;
    using Xunit;

    public class NestingDepthTests
    {
        private readonly IType elementType = Substitute.For<IType>();

        [Fact]
        public void TopLevelSet_ReturnsNull()
        {
            // Arrange
            var type = new SetType(this.elementType);

            // Act
            var nestingDepth = type.NestingDepth;

            // Assert
            Assert.Null(nestingDepth);
        }

        [Fact]
        public void SetIsNested_ReturnsOne()
        {
            // Arrange
            var type = new SetType(
                new SetType(this.elementType));

            // Act
            var nestingDepth = ((ICollectionType)type.ElementType).NestingDepth;

            // Assert
            Assert.Equal(1, nestingDepth);
        }

        [Fact]
        public void SetIsNestedMultipleLevelsDeep_ReturnsNestingLevel()
        {
            // Arrange
            // Simulate set<set<set<T>>>
            var type = new SetType(
                new SetType(
                    new SetType(this.elementType)));

            // Act
            var nestingDepth = ((ICollectionType)((ISetType)type.ElementType).ElementType).NestingDepth;

            // Assert
            Assert.Equal(2, nestingDepth);
        }

        [Fact]
        public void MixtureOfNestedTypes_ReturnsNestingLevel()
        {
            // Arrange
            // Simulate set<list<set<T>>>
            var type = new SetType(
                new ListType(
                    new SetType(this.elementType)));

            // Act
            var nestingDepth = ((ICollectionType)((ISetType)type.ElementType).ElementType).NestingDepth;

            // Assert
            Assert.Equal(2, nestingDepth);
        }
    }
}