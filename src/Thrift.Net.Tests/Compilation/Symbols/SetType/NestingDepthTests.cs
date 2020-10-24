namespace Thrift.Net.Tests.Compilation.Symbols.SetType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class NestingDepthTests
    {
        [Fact]
        public void TopLevelSet_ReturnsNull()
        {
            // Arrange
            var field = Substitute.For<ISymbol>();
            var type = new SetTypeBuilder()
                .SetParent(field)
                .Build();

            // Act
            var nestingDepth = type.NestingDepth;

            // Assert
            Assert.Null(nestingDepth);
        }

        [Fact]
        public void SetIsNested_ReturnsOne()
        {
            // Arrange
            var parent = Substitute.For<ISetType>();
            var type = new SetTypeBuilder()
                .SetParent(parent)
                .Build();

            // Act
            var nestingDepth = type.NestingDepth;

            // Assert
            Assert.Equal(1, nestingDepth);
        }

        [Fact]
        public void SetIsNestedMultipleLevelsDeep_ReturnsNestingLevel()
        {
            // Arrange
            // Simulate set<set<set<T>>>
            var field = Substitute.For<IField>();
            var type = new SetTypeBuilder() // set<>
                .SetParent(new SetTypeBuilder() // set<set<>>
                    .SetParent(new SetTypeBuilder() // set<set<set<>>>
                        .SetParent(field)
                        .Build())
                    .Build())
                .Build();

            // Act
            var nestingDepth = type.NestingDepth;

            // Assert
            Assert.Equal(2, nestingDepth);
        }

        [Fact]
        public void MixtureOfNestedTypes_ReturnsNestingLevel()
        {
            // Arrange
            // Simulate set<list<set<T>>>
            var field = Substitute.For<IField>();
            var type = new SetTypeBuilder() // set<>
                .SetParent(new ListTypeBuilder() // list<set<>>
                    .SetParent(new SetTypeBuilder() // set<list<set<>>>
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