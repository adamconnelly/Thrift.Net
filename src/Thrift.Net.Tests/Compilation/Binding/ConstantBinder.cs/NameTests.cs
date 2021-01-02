namespace Thrift.Net.Tests.Compilation.Binding.ConstantBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class NameTests
    {
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly ConstantBinder constantBinder;

        public NameTests()
        {
            this.constantBinder = new ConstantBinder(this.binderProvider);
        }

        [Fact]
        public void NameSupplied_SetsName()
        {
            // Arrange
            var input = ParserInput
                .FromString("const i32 MaxPageSize = 100")
                .ParseInput(parser => parser.constDefinition());
            var parent = Substitute.For<IDocument>();

            // Act
            var result = this.constantBinder.Bind<IConstant>(input, parent);

            // Assert
            Assert.Equal("MaxPageSize", result.Name);
        }

        [Fact]
        public void NameNotSupplied_Null()
        {
            // Arrange
            var input = ParserInput
                .FromString("const i32 = 100")
                .ParseInput(parser => parser.constDefinition());
            var parent = Substitute.For<IDocument>();

            // Act
            var result = this.constantBinder.Bind<IConstant>(input, parent);

            // Assert
            Assert.Null(result.Name);
        }
    }
}