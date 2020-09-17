namespace Thrift.Net.Tests.Compilation.Binding.DocumentBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;
    using static Thrift.Net.Antlr.ThriftParser;

    public class IsMemberAlreadyDeclaredTests : DocumentBinderTests
    {
        private readonly IBinder enumBinder = Substitute.For<IBinder>();
        private readonly IBinder structBinder = Substitute.For<IBinder>();

        [Fact]
        public void NoOtherMembersDeclared_ReturnsFalse()
        {
            var input = "enum UserType {}";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            this.SetupMember(documentNode.definitions().enumDefinition()[0], "UserType");

            // Act
            var isAlreadyDefined = this.Binder.IsMemberNameAlreadyDeclared(
                "UserType", documentNode.definitions().enumDefinition()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void HasDuplicateEnum_ReturnsTrue()
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
            var isAlreadyDefined = this.Binder.IsMemberNameAlreadyDeclared(
                "UserType", documentNode.definitions().enumDefinition()[1]);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void MemberDeclaredFirst_ReturnsFalse()
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
            var isAlreadyDefined = this.Binder.IsMemberNameAlreadyDeclared(
                "UserType", documentNode.definitions().enumDefinition()[0]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void MemberNotADuplicate_ReturnsFalse()
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
            var isAlreadyDefined = this.Binder.IsMemberNameAlreadyDeclared(
                "PermissionType", documentNode.definitions().enumDefinition()[1]);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void MultipleMemberTypesWithDuplicateNames_ReturnTrue()
        {
            var input =
@"enum UserType {}
struct UserType {}";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            this.SetupMember(documentNode.definitions().enumDefinition()[0], "UserType");
            this.SetupMember(documentNode.definitions().structDefinition()[0], "UserType");

            // Act
            var isAlreadyDefined = this.Binder.IsMemberNameAlreadyDeclared(
                "UserType", documentNode.definitions().structDefinition()[0]);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        private void SetupMember(EnumDefinitionContext enumNode, string enumName)
        {
            var member = new EnumBuilder()
                .SetNode(enumNode)
                .SetName(enumName)
                .Build();

            this.BinderProvider.GetBinder(enumNode).Returns(this.enumBinder);
            this.enumBinder.Bind<INamedSymbol>(enumNode).Returns(member);
        }

        private void SetupMember(StructDefinitionContext structNode, string structName)
        {
            var member = new StructBuilder()
                .SetNode(structNode)
                .SetName(structName)
                .Build();

            this.BinderProvider.GetBinder(structNode).Returns(this.structBinder);
            this.structBinder.Bind<INamedSymbol>(structNode).Returns(member);
        }
    }
}