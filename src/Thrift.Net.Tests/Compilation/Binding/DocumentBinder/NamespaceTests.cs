namespace Thrift.Net.Tests.Compilation.Binding.DocumentBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class NamespaceTests : DocumentBinderTests
    {
        [Fact]
        public void DocumentHasNoNamespaces_Empty()
        {
            // Arrange
            var input = string.Empty;

            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            // Act
            var document = this.Binder.Bind<Document>(documentNode);

            // Assert
            Assert.Empty(document.Namespaces);
        }

        [Fact]
        public void NamespacesProvided_SetsNamespaces()
        {
            // Arrange
            var input =
@"namespace * All
namespace csharp Thrift.Net.Examples";

            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            var namespaceBinder = Substitute.For<IBinder>();
            this.BinderProvider.GetBinder(default).ReturnsForAnyArgs(namespaceBinder);

            var allNamespace = new NamespaceBuilder().Build();
            namespaceBinder
                .Bind<Namespace>(documentNode.header().namespaceStatement()[0])
                .Returns(allNamespace);
            var csharpNamespace = new NamespaceBuilder().Build();
            namespaceBinder
                .Bind<Namespace>(documentNode.header().namespaceStatement()[1])
                .Returns(csharpNamespace);

            // Act
            var result = this.Binder.Bind<Document>(documentNode);

            // Assert
            Assert.Collection(
                result.Namespaces,
                all => Assert.Same(allNamespace, all),
                csharp => Assert.Same(csharpNamespace, csharp));
        }
    }
}