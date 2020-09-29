namespace Thrift.Net.Tests.Compilation.Binding.NamespaceBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class ScopeTests
    {
        [Fact]
        public void ScopeProvided_SetsScope()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var parentBinder = Substitute.For<IBinder>();
            var binderProvider = Substitute.For<IBinderProvider>();
            var binder = new NamespaceBinder(parentBinder, binderProvider);
            var namespaceStatement = ParserInput
                .FromString("namespace * Thrift.Net.Examples")
                .ParseInput(parser => parser.namespaceStatement());

            // Act
            var @namespace = binder.Bind<Namespace>(namespaceStatement, document);

            // Assert
            Assert.Equal("*", @namespace.Scope);
        }

        [Fact]
        public void ScopeNotProvided_Null()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var parentBinder = Substitute.For<IBinder>();
            var binderProvider = Substitute.For<IBinderProvider>();
            var binder = new NamespaceBinder(parentBinder, binderProvider);
            var namespaceStatement = ParserInput
                .FromString("namespace Thrift.Net.Examples")
                .ParseInput(parser => parser.namespaceStatement());

            // Act
            var @namespace = binder.Bind<Namespace>(namespaceStatement, document);

            // Assert
            Assert.Null(@namespace.Scope);
        }
    }
}