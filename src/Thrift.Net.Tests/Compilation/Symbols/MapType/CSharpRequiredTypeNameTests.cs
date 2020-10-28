namespace Thrift.Net.Tests.Compilation.Symbols.MapType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class CSharpRequiredTypeNameTests
    {
        private readonly IField field = Substitute.For<IField>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();

        [Fact]
        public void KeyAndValuesProvided_IncludesBothInType()
        {
            // Arrange
            var node = ParserInput
                .FromString("map<string, bool>")
                .ParseInput(parser => parser.mapType());

            var mapType = new MapTypeBuilder()
                .SetNode(node)
                .SetParent(this.field)
                .SetBinderProvider(this.binderProvider)
                .Build();

            var binder = Substitute.For<IBinder>();
            this.binderProvider.GetBinder(default).ReturnsForAnyArgs(binder);

            var keyType = Substitute.For<IFieldType>();
            keyType.CSharpRequiredTypeName.Returns("string");
            binder.Bind<IFieldType>(node.keyType, mapType).Returns(keyType);

            var valueType = Substitute.For<IFieldType>();
            valueType.CSharpRequiredTypeName.Returns("bool");
            binder.Bind<IFieldType>(node.valueType, mapType).Returns(valueType);

            // Act
            var name = mapType.CSharpRequiredTypeName;

            // Assert
            Assert.Equal("System.Collections.Generic.Dictionary<string, bool>", name);
        }

        [Fact]
        public void KeyTypeMissing_ReturnsNull()
        {
            // Arrange
            var node = ParserInput
                .FromString("map<, bool>")
                .ParseInput(parser => parser.mapType());

            var mapType = new MapTypeBuilder()
                .SetNode(node)
                .SetParent(this.field)
                .SetBinderProvider(this.binderProvider)
                .Build();

            var binder = Substitute.For<IBinder>();
            this.binderProvider.GetBinder(default).ReturnsForAnyArgs(binder);

            var valueType = Substitute.For<IFieldType>();
            valueType.CSharpRequiredTypeName.Returns("bool");
            binder.Bind<IFieldType>(node.valueType, mapType).Returns(valueType);

            // Act
            var name = mapType.CSharpRequiredTypeName;

            // Assert
            Assert.Null(name);
        }

        [Fact]
        public void ValueTypeMissing_ReturnsNull()
        {
            // Arrange
            var node = ParserInput
                .FromString("map<string,>")
                .ParseInput(parser => parser.mapType());

            var mapType = new MapTypeBuilder()
                .SetNode(node)
                .SetParent(this.field)
                .SetBinderProvider(this.binderProvider)
                .Build();

            var binder = Substitute.For<IBinder>();
            this.binderProvider.GetBinder(default).ReturnsForAnyArgs(binder);

            var keyType = Substitute.For<IFieldType>();
            keyType.CSharpRequiredTypeName.Returns("string");
            binder.Bind<IFieldType>(node.keyType, mapType).Returns(keyType);

            // Act
            var name = mapType.CSharpRequiredTypeName;

            // Assert
            Assert.Null(name);
        }
    }
}