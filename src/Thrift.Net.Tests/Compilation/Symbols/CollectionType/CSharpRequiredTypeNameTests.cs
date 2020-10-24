namespace Thrift.Net.Tests.Compilation.Symbols.CollectionType
{
    using System;
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Xunit;

    /// <summary>
    /// Contains tests for <see cref="CollectionType{TNode}.CSharpRequiredTypeName" />.
    /// </summary>
    public abstract partial class CollectionTypeTests
    {
        [Fact]
        public void CSharpRequiredTypeName_ElementExists_IncludesElementTypeInName()
        {
            // Arrange
            var (elementNode, collectionType) = this.ParseInput($"{this.collectionTypeName}<string>");

            var elementType = Substitute.For<IFieldType>();
            elementType.CSharpRequiredTypeName.Returns("string");

            this.binderProvider.GetBinder(elementNode).Returns(this.elementBinder);
            this.elementBinder.Bind<IFieldType>(elementNode, collectionType).Returns(elementType);

            // Act
            var name = collectionType.CSharpRequiredTypeName;

            // Assert
            Assert.Equal($"{this.csharpCollectionTypeName}<string>", name);
        }

        [Fact]
        public void ElementNotSpecified_ThrowsException()
        {
            // Arrange
            var (elementNode, collectionType) = this.ParseInput($"{this.collectionTypeName}<>");

            this.binderProvider.GetBinder(elementNode).Returns(this.elementBinder);
            this.elementBinder.Bind<IFieldType>(elementNode, collectionType).Returns((IFieldType)null);

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => collectionType.CSharpRequiredTypeName);
        }
    }
}