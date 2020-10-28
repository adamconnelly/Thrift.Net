namespace Thrift.Net.Tests.Compilation.Symbols.MapType
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

        [Fact]
        public void KeyAndValuesProvided_IncludesBothInName()
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

            // Act
            var name = mapType.Name;

            // Assert
            Assert.Equal("map<string,bool>", name);
        }

        [Fact]
        public void KeyNotProvided_DoesNotIncludeInName()
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

            // Act
            var name = mapType.Name;

            // Assert
            Assert.Equal("map<,bool>", name);
        }

        [Fact]
        public void ValueNotProvided_DoesNotIncludeInName()
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

            // Act
            var name = mapType.Name;

            // Assert
            Assert.Equal("map<string,>", name);
        }
    }
}