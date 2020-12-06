namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using Xunit;

    public class ExceptionTests : DocumentTests
    {
        [Fact]
        public void DocumentHasNoExceptions_Empty()
        {
            // Arrange
            var document = this.CreateDocumentFromInput(string.Empty);

            // Act
            var exceptions = document.Exceptions;

            // Assert
            Assert.Empty(exceptions);
        }

        [Fact]
        public void ExceptionsProvided_SetsExceptions()
        {
            // Arrange
            var input =
@"exception User {}
exception Address {}";
            var document = this.CreateDocumentFromInput(input);

            var notFoundException = this.SetupMember(
                document.Node.definitions().exceptionDefinition()[0],
                "NotFoundException",
                document);

            var invalidException = this.SetupMember(
                document.Node.definitions().exceptionDefinition()[1],
                "InvalidException",
                document);

            // Act
            var exceptions = document.Exceptions;

            // Assert
            Assert.Collection(
                exceptions,
                notFound => Assert.Same(notFoundException, notFound),
                invalid => Assert.Same(invalidException, invalid));
        }
    }
}