namespace Thrift.Net.Tests.Compilation.Binding.NamespaceBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class NamespaceTests
    {
        [Fact]
        public void NamespaceProvided_SetsNamespace()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var binderProvider = Substitute.For<IBinderProvider>();
            var binder = new NamespaceBinder(binderProvider);
            var namespaceStatement = ParserInput
                .FromString("namespace * Thrift.Net.Examples")
                .ParseInput(parser => parser.namespaceStatement());

            // Act
            var @namespace = binder.Bind<Namespace>(namespaceStatement, document);

            // Assert
            Assert.Equal("Thrift.Net.Examples", @namespace.NamespaceName);
        }

        [Fact]
        public void NamespaceNotProvided_Null()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var binderProvider = Substitute.For<IBinderProvider>();
            var binder = new NamespaceBinder(binderProvider);
            var namespaceStatement = ParserInput
                .FromString("namespace csharp")
                .ParseInput(parser => parser.namespaceStatement());

            // Act
            var @namespace = binder.Bind<Namespace>(namespaceStatement, document);

            // Assert
            Assert.Null(@namespace.NamespaceName);
        }
    }
}