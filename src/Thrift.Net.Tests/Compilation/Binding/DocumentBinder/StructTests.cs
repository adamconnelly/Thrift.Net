namespace Thrift.Net.Tests.Compilation.Binding.DocumentBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class StructTests : DocumentBinderTests
    {
        [Fact]
        public void DocumentHasNoStructs_Empty()
        {
            // Arrange
            var input = string.Empty;

            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            // Act
            var document = this.Binder.Bind<Document>(documentNode);

            // Assert
            Assert.Empty(document.Structs);
        }

        [Fact]
        public void StructsProvided_SetsStructs()
        {
            // Arrange
            var input =
@"struct User {}
struct Address {}";

            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            var structBinder = Substitute.For<IBinder>();
            this.BinderProvider.GetBinder(default).ReturnsForAnyArgs(structBinder);

            var userStruct = new StructBuilder().Build();
            structBinder
                .Bind<Struct>(documentNode.definitions().structDefinition()[0])
                .Returns(userStruct);
            var addressStruct = new StructBuilder().Build();
            structBinder
                .Bind<Struct>(documentNode.definitions().structDefinition()[1])
                .Returns(addressStruct);

            // Act
            var result = this.Binder.Bind<Document>(documentNode);

            // Assert
            Assert.Collection(
                result.Structs,
                user => Assert.Same(userStruct, user),
                address => Assert.Same(addressStruct, address));
        }
    }
}