namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Xunit;

    public class UnionTests : DocumentTests
    {
        [Fact]
        public void DocumentHasNoUnions_Empty()
        {
            // Arrange
            var document = this.CreateDocumentFromInput(string.Empty);

            // Act
            var unions = document.Unions;

            // Assert
            Assert.Empty(unions);
        }

        [Fact]
        public void UnionsProvided_SetsUnions()
        {
            // Arrange
            var input =
@"union User {}
union Address {}";
            var document = this.CreateDocumentFromInput(input);

            var userUnion = this.SetupMember(
                document.Node.definitions().unionDefinition()[0],
                "User",
                document);

            var addressUnion = this.SetupMember(
                document.Node.definitions().unionDefinition()[1],
                "Address",
                document);

            // Act
            var structs = document.Unions;

            // Assert
            Assert.Collection(
                structs,
                user => Assert.Same(userUnion, user),
                address => Assert.Same(addressUnion, address));
        }
    }
}