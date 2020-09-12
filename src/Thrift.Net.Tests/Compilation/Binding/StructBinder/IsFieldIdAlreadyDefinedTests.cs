namespace Thrift.Net.Tests.Compilation.Binding.StructBinder
{
    using Antlr4.Runtime.Tree;
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class IsFieldIdAlreadyDefinedTests : StructBinderTests
    {
        private readonly IBinder fieldBinder = Substitute.For<IBinder>();

        public IsFieldIdAlreadyDefinedTests()
        {
            this.BinderProvider.GetBinder(default).ReturnsForAnyArgs(this.fieldBinder);
        }

        [Fact]
        public void SingleField_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    0: string Username
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            this.SetupFieldWithId(structNode.field()[0], 0);

            // Act
            var isAlreadyDefined = this.Binder.IsFieldIdAlreadyDefined(
                0, structNode.field()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void DuplicateField_ReturnsTrue()
        {
            // Arrange
            var input =
@"struct User {
    1: i32 Id
    1: string Username
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            this.SetupFieldWithId(structNode.field()[0], 1);
            this.SetupFieldWithId(structNode.field()[1], 1);

            // Act
            var isAlreadyDefined = this.Binder.IsFieldIdAlreadyDefined(
                1, structNode.field()[1]);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void NoDuplicates_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    0: i32 Id
    1: string Username
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            this.SetupFieldWithId(structNode.field()[0], 0);
            this.SetupFieldWithId(structNode.field()[1], 1);

            // Act
            var isAlreadyDefined = this.Binder.IsFieldIdAlreadyDefined(
                1, structNode.field()[1]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void FirstFieldChosen_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    1: string Username
    1: string Username
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            this.SetupFieldWithId(structNode.field()[0], 1);
            this.SetupFieldWithId(structNode.field()[1], 1);

            // Act
            var isAlreadyDefined = this.Binder.IsFieldIdAlreadyDefined(
                1, structNode.field()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        private void SetupFieldWithId(IParseTree node, int fieldId)
        {
            this.fieldBinder.Bind<Field>(node)
                .Returns(new FieldBuilder().SetFieldId(fieldId).Build());
        }
    }
}