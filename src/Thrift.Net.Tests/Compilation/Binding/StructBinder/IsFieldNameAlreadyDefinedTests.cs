namespace Thrift.Net.Tests.Compilation.Binding.StructBinder
{
    using Antlr4.Runtime.Tree;
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Model;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class IsFieldNameAlreadyDefinedTests : StructBinderTests
    {
        private readonly IBinder fieldBinder = Substitute.For<IBinder>();

        public IsFieldNameAlreadyDefinedTests()
        {
            this.BinderProvider.GetBinder(default).ReturnsForAnyArgs(this.fieldBinder);
        }

        [Fact]
        public void SingleField_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    string Username
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            this.SetupFieldWithName(structNode.field()[0], "Username");

            // Act
            var isAlreadyDefined = this.Binder.IsFieldNameAlreadyDefined(
                "Username", structNode.field()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void DuplicateField_ReturnsTrue()
        {
            // Arrange
            var input =
@"struct User {
    string Username
    string Username
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            this.SetupFieldWithName(structNode.field()[0], "Username");
            this.SetupFieldWithName(structNode.field()[1], "Username");

            // Act
            var isAlreadyDefined = this.Binder.IsFieldNameAlreadyDefined(
                "Username", structNode.field()[1]);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void NoDuplicates_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
    string Username
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            this.SetupFieldWithName(structNode.field()[0], "Id");
            this.SetupFieldWithName(structNode.field()[1], "Username");

            // Act
            var isAlreadyDefined = this.Binder.IsFieldNameAlreadyDefined(
                "Username", structNode.field()[1]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void FirstFieldChosen_ReturnsFalse()
        {
            // Arrange
            var input =
@"struct User {
    string Username
    string Username
}";
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            this.SetupFieldWithName(structNode.field()[0], "Username");
            this.SetupFieldWithName(structNode.field()[1], "Username");

            // Act
            var isAlreadyDefined = this.Binder.IsFieldNameAlreadyDefined(
                "Username", structNode.field()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        private void SetupFieldWithName(IParseTree node, string name)
        {
            this.fieldBinder.Bind<FieldDefinition>(node)
                .Returns(this.CreateFieldWithName(name));
        }

        private FieldDefinition CreateFieldWithName(string name)
        {
            return new FieldDefinition(0, "0", FieldRequiredness.Default, FieldType.Binary, name);
        }
    }
}