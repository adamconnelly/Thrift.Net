namespace Thrift.Net.Tests.Compilation.Binding.FieldBinder
{
    using System.Collections.Generic;
    using System.Linq;
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Model;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class FieldTypeBinderTests
    {
        private readonly IBinder parentBinder = Substitute.For<IBinder>();
        private readonly FieldTypeBinder binder;

        public FieldTypeBinderTests()
        {
            this.binder = new FieldTypeBinder(this.parentBinder);
        }

        public static IEnumerable<object[]> GetBaseTypes()
        {
            return FieldType.BaseTypes.Select(type => new object[] { type });
        }

        [Theory]
        [MemberData(nameof(GetBaseTypes))]
        public void Bind_BaseType_ResolvesType(FieldType expectedType)
        {
            // Arrange
            var fieldTypeContext = ParserInput
                .FromString(expectedType.Name)
                .ParseInput(parser => parser.fieldType());

            // Act
            var type = this.binder.Bind<FieldType>(fieldTypeContext);

            // Assert
            Assert.Same(expectedType, type);
        }

        [Fact]
        public void Bind_NotBaseType_ResolvesTypeUsingParent()
        {
            // Arrange
            var input = "UserType";
            var fieldTypeContext = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.fieldType());

            var resolvedType = FieldType.CreateResolvedType(input);
            this.parentBinder.ResolveType(input).Returns(resolvedType);

            // Act
            var type = this.binder.Bind<FieldType>(fieldTypeContext);

            // Assert
            Assert.Same(resolvedType, type);
        }

        [Fact]
        public void Bind_TypeCannotResolve_ReturnsUnresolvedType()
        {
            // Arrange
            var input = "NonExistentType";
            var fieldTypeContext = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.fieldType());

            // Act
            var type = this.binder.Bind<FieldType>(fieldTypeContext);

            // Assert
            Assert.False(type.IsResolved);
        }

        [Theory]
        [InlineData("i32", 1)]
        [InlineData("Enums.UserType", 2)]
        [InlineData("One.Two.Three", 3)]
        public void Bind_MultiPartIdentifier_SetsIdentifierPartCount(
            string identifier, int parts)
        {
            // Arrange
            var fieldTypeContext = ParserInput
                .FromString(identifier)
                .ParseInput(parser => parser.fieldType());

            // Act
            var type = this.binder.Bind<FieldType>(fieldTypeContext);

            // Assert
            Assert.Equal(parts, type.IdentifierPartsCount);
        }

        [Fact]
        public void Bind_MultiPartIdentifierWithMoreThanTwoParts_DoesNotResolveTypeUsingParent()
        {
            // Thrift only support simple names, or names with a single prefix,
            // so don't bother trying to resolve a type if it has more than two
            // parts.
            //
            // Arrange
            var fieldTypeContext = ParserInput
                .FromString("One.Two.Three")
                .ParseInput(parser => parser.fieldType());

            // Act
            this.binder.Bind<FieldType>(fieldTypeContext);

            // Assert
            this.parentBinder.DidNotReceive().ResolveType(Arg.Any<string>());
        }
    }
}