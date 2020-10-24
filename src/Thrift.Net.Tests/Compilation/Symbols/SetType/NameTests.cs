namespace Thrift.Net.Tests.Compilation.Symbols
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
                .FromString("set<string>")
                .ParseInput(parser => parser.setType());

            var setType = new SetTypeBuilder()
                .SetNode(node)
                .SetParent(this.field)
                .SetBinderProvider(this.binderProvider)
                .Build();

            var elementType = Substitute.For<IFieldType>();
            elementType.Name.Returns("string");

            this.binderProvider.GetBinder(node.fieldType()).Returns(this.elementBinder);
            this.elementBinder.Bind<IFieldType>(node.fieldType(), setType).Returns(elementType);

            // Act
            var name = setType.Name;

            // Assert
            Assert.Equal("set<string>", name);
        }

        [Fact]
        public void ElementNotSpecified_DoesNotIncludeElementTypeInName()
        {
            // Arrange
            var node = ParserInput
                .FromString("set<>")
                .ParseInput(parser => parser.setType());

            var setType = new SetTypeBuilder()
                .SetNode(node)
                .SetParent(this.field)
                .SetBinderProvider(this.binderProvider)
                .Build();

            this.binderProvider.GetBinder(node.fieldType()).Returns(this.elementBinder);
            this.elementBinder.Bind<IFieldType>(node.fieldType(), setType).Returns((IFieldType)null);

            // Act
            var name = setType.Name;

            // Assert
            Assert.Equal("set<>", name);
        }
    }
}