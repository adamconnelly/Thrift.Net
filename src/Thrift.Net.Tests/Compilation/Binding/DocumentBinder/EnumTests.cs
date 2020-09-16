namespace Thrift.Net.Tests.Compilation.Binding.DocumentBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class EnumTests : DocumentBinderTests
    {
        [Fact]
        public void DocumentHasNoEnums_Empty()
        {
            // Arrange
            var input = string.Empty;

            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            // Act
            var document = this.Binder.Bind<Document>(documentNode);

            // Assert
            Assert.Empty(document.Enums);
        }

        [Fact]
        public void EnumsProvided_SetsEnums()
        {
            // Arrange
            var input =
@"enum UserType {}
enum PermissionType {}";

            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            var enumBinder = Substitute.For<IBinder>();
            this.BinderProvider.GetBinder(default).ReturnsForAnyArgs(enumBinder);

            var userTypeEnum = new EnumBuilder().Build();
            enumBinder
                .Bind<Enum>(documentNode.definitions().enumDefinition()[0])
                .Returns(userTypeEnum);
            var permissionEnum = new EnumBuilder().Build();
            enumBinder
                .Bind<Enum>(documentNode.definitions().enumDefinition()[1])
                .Returns(permissionEnum);

            // Act
            var result = this.Binder.Bind<Document>(documentNode);

            // Assert
            Assert.Collection(
                result.Enums,
                userType => Assert.Same(userTypeEnum, userType),
                permissionEnum => Assert.Same(permissionEnum, permissionEnum));
        }
    }
}