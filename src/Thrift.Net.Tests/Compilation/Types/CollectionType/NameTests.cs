namespace Thrift.Net.Tests.Compilation.Types.CollectionType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Types;
    using Xunit;

    /// <summary>
    /// Contains tests for <see cref="CollectionType{TNode}.Name" />.
    /// </summary>
    public abstract partial class CollectionTypeTests
    {
        [Fact]
        public void Name_ElementExists_IncludesElementTypeInName()
        {
            // Arrange
            var elementType = Substitute.For<IType>();
            elementType.Name.Returns("string");

            var collectionType = this.CreateCollectionType(elementType);

            // Act
            var name = collectionType.Name;

            // Assert
            Assert.Equal($"{this.collectionTypeName}<string>", name);
        }

        [Fact]
        public void Name_ElementNotSpecified_DoesNotIncludeElementTypeInName()
        {
            // Arrange
            var collectionType = this.CreateCollectionType(null);

            // Act
            var name = collectionType.Name;

            // Assert
            Assert.Equal($"{this.collectionTypeName}<>", name);
        }
    }
}