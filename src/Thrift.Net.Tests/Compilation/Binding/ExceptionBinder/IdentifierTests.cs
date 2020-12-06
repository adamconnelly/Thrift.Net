namespace Thrift.Net.Tests.Compilation.Binding.ExceptionBinder
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

        private readonly ExceptionBinder binder;

        public IdentifierTests()
        {
            this.binder = new ExceptionBinder(this.binderProvider);
        }

        [Fact]
        public void Bind_ExceptionNameSupplied_SetsName()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var exceptionNode = ParserInput
                .FromString("exception NotFoundException {}")
                .ParseInput(parser => parser.exceptionDefinition());

            // Act
            var exception = this.binder.Bind<IException>(exceptionNode, document);

            // Assert
            Assert.Equal("NotFoundException", exception.Name);
        }

        [Fact]
        public void Bind_ExceptionNameNotSupplied_Null()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var exceptionNode = ParserInput
                .FromString("exception {}")
                .ParseInput(parser => parser.exceptionDefinition());

            // Act
            var exception = this.binder.Bind<IException>(exceptionNode, document);

            // Assert
            Assert.Null(exception.Name);
        }
    }
}