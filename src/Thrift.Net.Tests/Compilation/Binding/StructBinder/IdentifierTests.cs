namespace Thrift.Net.Tests.Compilation.Binding.StructBinder
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class IdentifierTests : StructBinderTests
    {
        [Fact]
        public void Bind_StructNameSupplied_SetsName()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var structContext = ParserInput
                .FromString("struct User {}")
                .ParseInput(parser => parser.structDefinition());

            // Act
            var structDefinition = this.Binder.Bind<Struct>(structContext, document);

            // Assert
            Assert.Equal("User", structDefinition.Name);
        }

        [Fact]
        public void Bind_StructNameNotSupplied_Null()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var structContext = ParserInput
                .FromString("struct {}")
                .ParseInput(parser => parser.structDefinition());

            // Act
            var structDefinition = this.Binder.Bind<Struct>(structContext, document);

            // Assert
            Assert.Null(structDefinition.Name);
        }
    }
}