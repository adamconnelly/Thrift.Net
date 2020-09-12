namespace Thrift.Net.Tests.Compilation.Binding.FieldBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class RequirednessTests
    {
        private readonly IFieldContainerBinder containerBinder = Substitute.For<IFieldContainerBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly FieldBinder binder;

        public RequirednessTests()
        {
            this.binder = new FieldBinder(this.containerBinder, this.binderProvider);
        }

        [Fact]
        public void Bind_FieldRequirednessNotSupplied_UsesDefault()
        {
            // Arrange
            this.containerBinder.DefaultFieldRequiredness
                .Returns(FieldRequiredness.Optional);

            var fieldContext = ParserInput
                .FromString("i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Equal(FieldRequiredness.Optional, field.Requiredness);
        }

        [Fact]
        public void Bind_FieldRequirednessRequired_SetsRequiredness()
        {
            // Arrange
            var fieldContext = ParserInput
                .FromString("required i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Equal(FieldRequiredness.Required, field.Requiredness);
        }

        [Fact]
        public void Bind_FieldRequirednessOptional_SetsRequiredness()
        {
            // Arrange
            var fieldContext = ParserInput
                .FromString("optional i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Equal(FieldRequiredness.Optional, field.Requiredness);
        }
    }
}