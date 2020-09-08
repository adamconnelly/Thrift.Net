namespace Thrift.Net.Tests.Compilation.Binding.StructBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Model;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class GetPreviousSiblingTests : StructBinderTests
    {
        [Fact]
        public void GetPreviousSibling_SiblingExists_ReturnsField()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id,
    string User
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            var fieldBinder = Substitute.For<IBinder>();
            var fieldDefinition = new FieldDefinition(
                0, "0", FieldRequiredness.Default, FieldType.I32, "Id");
            fieldBinder.Bind<FieldDefinition>(structNode.field()[0])
                .Returns(fieldDefinition);

            this.BinderProvider.GetBinder(structNode.field()[0]).Returns(fieldBinder);

            // Act
            var sibling = this.Binder.GetPreviousSibling(structNode.field()[1]);

            // Assert
            Assert.Same(fieldDefinition, sibling);
        }

        [Fact]
        public void GetPreviousSibling_NodeHasNoSiblings_ReturnsNull()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            var fieldBinder = Substitute.For<IBinder>();
            var fieldDefinition = new FieldDefinition(
                0, "0", FieldRequiredness.Default, FieldType.I32, "Id");
            fieldBinder.Bind<FieldDefinition>(default)
                .ReturnsForAnyArgs(fieldDefinition);

            this.BinderProvider.GetBinder(default).ReturnsForAnyArgs(fieldBinder);

            // Act
            var sibling = this.Binder.GetPreviousSibling(structNode.field()[0]);

            // Assert
            Assert.Null(sibling);
        }

        [Fact]
        public void GetPreviousSibling_NodeIsFirstSibling_ReturnsNull()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id,
    string Username
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            var fieldBinder = Substitute.For<IBinder>();
            var fieldDefinition = new FieldDefinition(
                0, "0", FieldRequiredness.Default, FieldType.I32, "Id");
            fieldBinder.Bind<FieldDefinition>(default)
                .ReturnsForAnyArgs(fieldDefinition);

            this.BinderProvider.GetBinder(default).ReturnsForAnyArgs(fieldBinder);

            // Act
            var sibling = this.Binder.GetPreviousSibling(structNode.field()[0]);

            // Assert
            Assert.Null(sibling);
        }
    }
}