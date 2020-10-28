namespace Thrift.Net.Tests.Compilation.Binding.FieldTypeBinder
{
    using System;
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

        [Fact]
        public void Bind_ListType_ResolvesType()
        {
            // Arrange
            var field = new FieldBuilder().Build();
            var node = ParserInput
                .FromString("list<string>")
                .ParseInput(parser => parser.fieldType());

            this.binderProvider.GetBinder(node.collectionType().listType())
                .Returns(this.typeBinder);
            var listType = Substitute.For<IListType>();
            this.typeBinder.Bind<IFieldType>(node.collectionType().listType(), field)
                .Returns(listType);

            // Act
            var type = this.binder.Bind<IFieldType>(node, field);

            // Assert
            Assert.Same(listType, type);
        }

        [Fact]
        public void Bind_SetType_ResolvesType()
        {
            // Arrange
            var field = new FieldBuilder().Build();
            var node = ParserInput
                .FromString("set<string>")
                .ParseInput(parser => parser.fieldType());

            this.binderProvider.GetBinder(node.collectionType().setType())
                .Returns(this.typeBinder);
            var setType = Substitute.For<ISetType>();
            this.typeBinder.Bind<IFieldType>(node.collectionType().setType(), field)
                .Returns(setType);

            // Act
            var type = this.binder.Bind<IFieldType>(node, field);

            // Assert
            Assert.Same(setType, type);
        }

        [Fact]
        public void Bind_MapType_ResolvesType()
        {
            // Arrange
            var field = new FieldBuilder().Build();
            var node = ParserInput
                .FromString("map<string, string>")
                .ParseInput(parser => parser.fieldType());

            this.binderProvider.GetBinder(node.collectionType().mapType())
                .Returns(this.typeBinder);
            var mapType = Substitute.For<IMapType>();
            this.typeBinder.Bind<IFieldType>(node.collectionType().mapType(), field)
                .Returns(mapType);

            // Act
            var type = this.binder.Bind<IFieldType>(node, field);

            // Assert
            Assert.Same(mapType, type);
        }

        [Fact]
        public void Bind_UnknownType_ThrowsException()
        {
            // Arrange
            var field = new FieldBuilder().Build();
            var node = ParserInput
                .FromString("12345")
                .ParseInput(parser => parser.fieldType());

            // Act / Assert
            Assert.Throws<ArgumentException>(() => this.binder.Bind<IFieldType>(node, field));
        }
    }
}