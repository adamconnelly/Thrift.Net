namespace Thrift.Net.Tests.Compilation.Binding.FieldBinder
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
        private readonly FieldBinder binder;

        public IdentifierTests()
        {
            this.binder = new FieldBinder(this.binderProvider);
        }

        [Fact]
        public void Bind_IdentifierSupplied_SetsIdentifier()
        {
            // Arrange
            var @struct = new StructBuilder().Build();
            var fieldContext = ParserInput
                .FromString("string Name")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext, @struct);

            // Assert
            Assert.Equal("Name", field.Name);
        }
    }
}