namespace Thrift.Net.Tests.Compilation.Binding.FieldBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class FieldTypeTests
    {
        private readonly IFieldContainerBinder containerBinder = Substitute.For<IFieldContainerBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly IBinder typeBinder = Substitute.For<IBinder>();
        private readonly FieldBinder binder;

        public FieldTypeTests()
        {
            this.binder = new FieldBinder(this.containerBinder, this.binderProvider);
        }

        [Fact]
        public void Bind_FieldTypeSupplied_UsesBinderToResolveType()
        {
            // Arrange
            var fieldContext = ParserInput
                .FromString("i32 Id")
                .ParseInput(parser => parser.field());

            this.binderProvider.GetBinder(fieldContext.fieldType())
                .Returns(this.typeBinder);
            this.typeBinder.Bind<FieldType>(fieldContext.fieldType())
                .Returns(FieldType.Bool);

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Same(FieldType.Bool, field.Type);
        }
    }
}