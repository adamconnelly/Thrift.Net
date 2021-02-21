namespace Thrift.Net.Tests.Compilation.Types.CollectionType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Types;
    using Xunit;

    /// <summary>
    /// Contains tests for <see cref="CollectionType{TNode}.CSharpOptionalTypeName" />.
    /// </summary>
    public abstract partial class CollectionTypeTests
    {
        [Fact]
        public void CSharpOptionalTypeName_ElementExists_IncludesElementTypeInName()
        {
            // Arrange
            var elementType = Substitute.For<IType>();
            elementType.CSharpRequiredTypeName.Returns("string");
            var collectionType = this.CreateCollectionType(elementType);

            // Act
            var name = collectionType.CSharpOptionalTypeName;

            // Assert
            Assert.Equal($"{this.csharpCollectionTypeName}<string>", name);
        }

        [Fact]
        public void CSharpOptionalTypeName_ElementUnresolved_ThrowsException()
        {
            // TODO: Figure out how we're gonna deal with unresolved types
        }
    }
}