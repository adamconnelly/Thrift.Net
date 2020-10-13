namespace Thrift.Net.Tests.Compilation.Symbols.ListType
{
    using System;
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class CSharpOptionalTypeNameTests
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
            elementType.CSharpRequiredTypeName.Returns("string");

            this.binderProvider.GetBinder(node.fieldType()).Returns(this.elementBinder);
            this.elementBinder.Bind<IFieldType>(node.fieldType(), listType).Returns(elementType);

            // Act
            var name = listType.CSharpOptionalTypeName;

            // Assert
            Assert.Equal("System.Collections.Generic.List<string>", name);
        }

        [Fact]
        public void ElementNotSpecified_ThrowsException()
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

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => listType.CSharpOptionalTypeName);
        }
    }
}