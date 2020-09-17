namespace Thrift.Net.Tests.Compilation.Binding.DocumentBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;
    using static Thrift.Net.Antlr.ThriftParser;

    public class IsEnumAlreadyDeclaredTests : DocumentBinderTests
    {
        private readonly IBinder enumBinder = Substitute.For<IBinder>();

        [Fact]
        public void NoOtherEnumsDeclared_ReturnsFalse()
        {
            var input = "enum UserType {}";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            this.SetupMember(documentNode.definitions().enumDefinition()[0], "UserType");

            // Act
            var isAlreadyDefined = this.Binder.IsEnumAlreadyDeclared(
                "UserType", documentNode.definitions().enumDefinition()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void EnumIsADuplicate_ReturnsTrue()
        {
            var input =
@"enum UserType {}
enum UserType {}";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            this.SetupMember(documentNode.definitions().enumDefinition()[0], "UserType");
            this.SetupMember(documentNode.definitions().enumDefinition()[1], "UserType");

            // Act
            var isAlreadyDefined = this.Binder.IsEnumAlreadyDeclared(
                "UserType", documentNode.definitions().enumDefinition()[1]);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void EnumDeclaredFirst_ReturnsFalse()
        {
            var input =
@"enum UserType {}
enum UserType {}";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            this.SetupMember(documentNode.definitions().enumDefinition()[0], "UserType");
            this.SetupMember(documentNode.definitions().enumDefinition()[1], "UserType");

            // Act
            var isAlreadyDefined = this.Binder.IsEnumAlreadyDeclared(
                "UserType", documentNode.definitions().enumDefinition()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void EnumNotADuplicate_ReturnsFalse()
        {
            var input =
@"enum UserType {}
enum PermissionType {}";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            this.SetupMember(documentNode.definitions().enumDefinition()[0], "UserType");
            this.SetupMember(documentNode.definitions().enumDefinition()[1], "PermissionType");

            // Act
            var isAlreadyDefined = this.Binder.IsEnumAlreadyDeclared(
                "PermissionType", documentNode.definitions().enumDefinition()[1]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        private void SetupMember(EnumDefinitionContext enumNode, string enumName)
        {
            var member = new EnumBuilder()
                .SetNode(enumNode)
                .SetName(enumName)
                .Build();

            this.BinderProvider.GetBinder(enumNode).Returns(this.enumBinder);
            this.enumBinder.Bind<Enum>(enumNode).Returns(member);
        }
    }
}