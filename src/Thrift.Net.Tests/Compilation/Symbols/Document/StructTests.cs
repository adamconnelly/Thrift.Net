namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Xunit;

    public class StructTests : DocumentTests
    {
        [Fact]
        public void DocumentHasNoStructs_Empty()
        {
            // Arrange
            var document = this.CreateDocumentFromInput(string.Empty);

            // Act
            var structs = document.Structs;

            // Assert
            Assert.Empty(structs);
        }

        [Fact]
        public void StructsProvided_SetsStructs()
        {
            // Arrange
            var input =
@"struct User {}
struct Address {}";
            var document = this.CreateDocumentFromInput(input);

            var userStruct = this.SetupMember(
                document.Node.definitions().structDefinition()[0],
                "User",
                document);

            var addressStruct = this.SetupMember(
                document.Node.definitions().structDefinition()[1],
                "Address",
                document);

            // Act
            var structs = document.Structs;

            // Assert
            Assert.Collection(
                structs,
                user => Assert.Same(userStruct, user),
                address => Assert.Same(addressStruct, address));
        }
    }
}