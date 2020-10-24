namespace Thrift.Net.Tests.Compilation.Symbols.CollectionType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
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
            var (elementNode, listType) = this.ParseInput($"{this.collectionTypeName}<string>");

            var elementType = Substitute.For<IFieldType>();
            elementType.Name.Returns("string");

            this.binderProvider.GetBinder(elementNode).Returns(this.elementBinder);
            this.elementBinder.Bind<IFieldType>(elementNode, listType).Returns(elementType);

            // Act
            var name = listType.Name;

            // Assert
            Assert.Equal($"{this.collectionTypeName}<string>", name);
        }

        [Fact]
        public void ElementNotSpecified_DoesNotIncludeElementTypeInName()
        {
            // Arrange
            var (elementNode, listType) = this.ParseInput($"{this.collectionTypeName}<>");

            this.binderProvider.GetBinder(elementNode).Returns(this.elementBinder);
            this.elementBinder.Bind<IFieldType>(elementNode, listType).Returns((IFieldType)null);

            // Act
            var name = listType.Name;

            // Assert
            Assert.Equal($"{this.collectionTypeName}<>", name);
        }
    }
}