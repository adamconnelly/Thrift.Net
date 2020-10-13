namespace Thrift.Net.Tests.Compilation.Symbols.ListType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class NameTests
    {
        private readonly IField field = Substitute.For<IField>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly IBinder elementBinder = Substitute.For<IBinder>();

        [Fact]
        public void ElementExists_IncludesElementTypeInName()
        {
            // Arrange
            var node = ParserInput
                .FromString("list<string>")
                .ParseInput(parser => parser.listType());

            var listType = new ListTypeBuilder()
                .SetNode(node)
                .SetParent(this.field)
                .SetBinderProvider(this.binderProvider)
                .Build();

            var elementType = Substitute.For<IFieldType>();
            elementType.Name.Returns("string");

            this.binderProvider.GetBinder(node.fieldType()).Returns(this.elementBinder);
            this.elementBinder.Bind<IFieldType>(node.fieldType(), listType).Returns(elementType);

            // Act
            var name = listType.Name;

            // Assert
            Assert.Equal("list<string>", name);
        }

        [Fact]
        public void ElementNotSpecified_DoesNotIncludeElementTypeInName()
        {
            // Arrange
            var node = ParserInput
                .FromString("list<>")
                .ParseInput(parser => parser.listType());

            var listType = new ListTypeBuilder()
                .SetNode(node)
                .SetParent(this.field)
                .SetBinderProvider(this.binderProvider)
                .Build();

            this.binderProvider.GetBinder(node.fieldType()).Returns(this.elementBinder);
            this.elementBinder.Bind<IFieldType>(node.fieldType(), listType).Returns((IFieldType)null);

            // Act
            var name = listType.Name;

            // Assert
            Assert.Equal("list<>", name);
        }
    }
}