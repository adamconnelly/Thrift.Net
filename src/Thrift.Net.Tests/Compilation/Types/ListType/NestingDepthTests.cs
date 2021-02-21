namespace Thrift.Net.Tests.Compilation.Types.ListType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Types;
    using Xunit;

    using ListType = Thrift.Net.Compilation.Types.ListType;

    public class NestingDepthTests
    {
        private readonly IType elementType = Substitute.For<IType>();

        [Fact]
        public void TopLevelList_ReturnsNull()
        {
            // Arrange
            var type = new ListType(this.elementType);

            // Act
            var nestingDepth = type.NestingDepth;

            // Assert
            Assert.Null(nestingDepth);
        }

        [Fact]
        public void ListIsNested_ReturnsOne()
        {
            // Arrange
            var type = new ListType(this.elementType);

            // Act
            var nestingDepth = ((ICollectionType)type.ElementType).NestingDepth;

            // Assert
            Assert.Equal(1, nestingDepth);
        }

        [Fact]
        public void ListIsNestedMultipleLevelsDeep_ReturnsNestingLevel()
        {
            // Arrange
            // Simulate List<List<List<T>>>
            var type = new ListType(
                new ListType(
                    new ListType(
                        this.elementType)));

            // Act
            var nestingDepth = ((ICollectionType)((ISetType)type.ElementType).ElementType).NestingDepth;

            // Assert
            Assert.Equal(2, nestingDepth);
        }

        [Fact]
        public void MixtureOfNestedTypes_ReturnsNestingLevel()
        {
            // Arrange
            // Simulate list<set<list<T>>>
            var type = new ListType(
                new SetType(
                    new ListType(
                        this.elementType)));

            // Act
            var nestingDepth = ((ICollectionType)((ISetType)type.ElementType).ElementType).NestingDepth;

            // Assert
            Assert.Equal(2, nestingDepth);
        }
    }
}