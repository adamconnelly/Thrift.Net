namespace Thrift.Net.Tests.Compilation.Binding.DocumentBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;
    using static Thrift.Net.Antlr.ThriftParser;

    public class IsNamespaceForScopeAlreadySpecified : DocumentBinderTests
    {
        private readonly IBinder namespaceBinder = Substitute.For<IBinder>();

        [Fact]
        public void SingleNamespace_ReturnsFalse()
        {
            var input = "namespace csharp Thrift.Net.Examples";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            var @namespace = this.SetupNamespace(
                documentNode.header().namespaceStatement()[0],
                "csharp",
                "Thrift.Net.Examples");

            // Act
            var isAlreadyDefined = this.Binder.IsNamespaceForScopeAlreadyDeclared(
                @namespace);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void NamespaceScopeAlreadyDeclared_ReturnsTrue()
        {
            var input =
@"namespace csharp Thrift.Net.Examples.A
namespace csharp Thrift.Net.Examples.B";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            this.SetupNamespace(
                documentNode.header().namespaceStatement()[0],
                "csharp",
                "Thrift.Net.Examples.A");

            var @namespace = this.SetupNamespace(
                documentNode.header().namespaceStatement()[1],
                "csharp",
                "Thrift.Net.Examples.B");

            // Act
            var isAlreadyDefined = this.Binder.IsNamespaceForScopeAlreadyDeclared(
                @namespace);

            // Assert
            Assert.True(isAlreadyDefined);
        }

        [Fact]
        public void FirstDeclarationOfScope_ReturnsFalse()
        {
            var input =
@"namespace csharp Thrift.Net.Examples.A
namespace csharp Thrift.Net.Examples.B";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            var @namespace = this.SetupNamespace(
                documentNode.header().namespaceStatement()[0],
                "csharp",
                "Thrift.Net.Examples.A");

            this.SetupNamespace(
                documentNode.header().namespaceStatement()[1],
                "csharp",
                "Thrift.Net.Examples.B");

            // Act
            var isAlreadyDefined = this.Binder.IsNamespaceForScopeAlreadyDeclared(
                @namespace);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        [Fact]
        public void ScopeNotDuplicated_ReturnsFalse()
        {
            var input =
@"namespace csharp Thrift.Net.Examples.A
namespace netstd Thrift.Net.Examples.B";
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            this.SetupNamespace(
                documentNode.header().namespaceStatement()[0],
                "csharp",
                "Thrift.Net.Examples.A");

            var @namespace = this.SetupNamespace(
                documentNode.header().namespaceStatement()[1],
                "netstd",
                "Thrift.Net.Examples.B");

            // Act
            var isAlreadyDefined = this.Binder.IsNamespaceForScopeAlreadyDeclared(
                @namespace);

            // Assert
            Assert.False(isAlreadyDefined);
        }

        private Namespace SetupNamespace(NamespaceStatementContext namespaceNode, string scope, string name)
        {
            var @namespace = new NamespaceBuilder()
                .SetNode(namespaceNode)
                .SetScope(scope)
                .SetNamespaceName(name)
                .Build();

            this.BinderProvider.GetBinder(namespaceNode).Returns(this.namespaceBinder);
            this.namespaceBinder.Bind<Namespace>(namespaceNode).Returns(@namespace);

            return @namespace;
        }
    }
}