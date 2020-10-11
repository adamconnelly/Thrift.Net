namespace Thrift.Net.Tests.Compilation.Binding.FieldTypeBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class FieldTypeBinderTests
    {
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly IBinder typeBinder = Substitute.For<IBinder>();
        private readonly FieldTypeBinder binder;

        public FieldTypeBinderTests()
        {
            this.binder = new FieldTypeBinder(this.binderProvider);
        }

        [Fact]
        public void Bind_BaseType_ResolvesType()
        {
            // Arrange
            var field = new FieldBuilder().Build();
            var node = ParserInput
                .FromString("bool")
                .ParseInput(parser => parser.fieldType());

            this.binderProvider.GetBinder(node.baseType()).Returns(this.typeBinder);
            var baseType = Substitute.For<IBaseType>();
            this.typeBinder.Bind<IFieldType>(node.baseType(), field).Returns(baseType);

            // Act
            var type = this.binder.Bind<IFieldType>(node, field);

            // Assert
            Assert.Same(baseType, type);
        }

        [Fact]
        public void Bind_UserType_ResolvesType()
        {
            // Arrange
            var field = new FieldBuilder().Build();
            var node = ParserInput
                .FromString("User")
                .ParseInput(parser => parser.fieldType());

            this.binderProvider.GetBinder(node.userType()).Returns(this.typeBinder);
            var userType = Substitute.For<IUserType>();
            this.typeBinder.Bind<IFieldType>(node.userType(), field).Returns(userType);

            // Act
            var type = this.binder.Bind<IFieldType>(node, field);

            // Assert
            Assert.Same(userType, type);
        }
    }
}