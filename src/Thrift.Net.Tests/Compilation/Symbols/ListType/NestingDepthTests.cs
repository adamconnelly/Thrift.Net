namespace Thrift.Net.Tests.Compilation.Symbols.ListType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class NestingDepthTests
    {
        [Fact]
        public void TopLevelList_ReturnsNull()
        {
            // Arrange
            var field = Substitute.For<ISymbol>();
            var type = new ListTypeBuilder()
                .SetParent(field)
                .Build();

            // Act
            var nestingDepth = type.NestingDepth;

            // Assert
            Assert.Null(nestingDepth);
        }

        [Fact]
        public void ListIsNested_ReturnsOne()
        {
            // Arrange
            var parent = Substitute.For<IListType>();
            var type = new ListTypeBuilder()
                .SetParent(parent)
                .Build();

            // Act
            var nestingDepth = type.NestingDepth;

            // Assert
            Assert.Equal(1, nestingDepth);
        }

        [Fact]
        public void ListIsNestedMultipleLevelsDeep_ReturnsNestingLevel()
        {
            // Arrange
            // Simulate List<List<List<T>>>
            var field = Substitute.For<IField>();
            var type = new ListTypeBuilder() // List<>
                .SetParent(new ListTypeBuilder() // List<List<>>
                    .SetParent(new ListTypeBuilder() // List<List<List<>>>
                        .SetParent(field)
                        .Build())
                    .Build())
                .Build();

            // Act
            var nestingDepth = type.NestingDepth;

            // Assert
            Assert.Equal(2, nestingDepth);
        }
    }
}