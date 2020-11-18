namespace Thrift.Net.Tests.Compilation.Binding.FieldBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class FieldTypeTests
    {
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly IBinder typeBinder = Substitute.For<IBinder>();
        private readonly FieldBinder binder;

        public FieldTypeTests()
        {
            this.binder = new FieldBinder(this.binderProvider);
        }

        [Fact]
        public void Bind_FieldTypeSupplied_UsesBinderToResolveType()
        {
            // Arrange
            var @struct = new StructBuilder().Build();
            var input =
@"struct User {
    i32 Id
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            this.binderProvider.GetBinder(structNode.field()[0].fieldType())
                .Returns(this.typeBinder);
            var type = Substitute.For<IFieldType>();
            this.typeBinder.Bind<IFieldType>(
                structNode.field()[0].fieldType(), Arg.Any<ISymbol>())
                .Returns(type);

            // Act
            var field = this.binder.Bind<Field>(structNode.field()[0], @struct);

            // Assert
            Assert.Same(type, field.Type);
        }
    }
}