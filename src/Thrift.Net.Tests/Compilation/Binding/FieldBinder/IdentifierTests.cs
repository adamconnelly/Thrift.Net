namespace Thrift.Net.Tests.Compilation.Binding.FieldBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Model;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class IdentifierTests
    {
        private readonly IFieldContainerBinder containerBinder = Substitute.For<IFieldContainerBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly FieldBinder binder;

        public IdentifierTests()
        {
            this.binder = new FieldBinder(this.containerBinder, this.binderProvider);
        }

        [Fact]
        public void Bind_IdentifierSupplied_SetsIdentifier()
        {
            // Arrange
            var fieldContext = ParserInput
                .FromString("string Name")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<FieldDefinition>(fieldContext);

            // Assert
            Assert.Equal("Name", field.Name);
        }
    }
}