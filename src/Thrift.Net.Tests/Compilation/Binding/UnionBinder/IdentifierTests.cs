namespace Thrift.Net.Tests.Compilation.Binding.UnionBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class IdentifierTests
    {
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();

        private readonly UnionBinder binder;

        public IdentifierTests()
        {
            this.binder = new UnionBinder(this.binderProvider);
        }

        [Fact]
        public void Bind_StructNameSupplied_SetsName()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var structContext = ParserInput
                .FromString("union User {}")
                .ParseInput(parser => parser.unionDefinition());

            // Act
            var union = this.binder.Bind<IUnion>(structContext, document);

            // Assert
            Assert.Equal("User", union.Name);
        }

        [Fact]
        public void Bind_StructNameNotSupplied_Null()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var structContext = ParserInput
                .FromString("union {}")
                .ParseInput(parser => parser.unionDefinition());

            // Act
            var union = this.binder.Bind<IUnion>(structContext, document);

            // Assert
            Assert.Null(union.Name);
        }
    }
}