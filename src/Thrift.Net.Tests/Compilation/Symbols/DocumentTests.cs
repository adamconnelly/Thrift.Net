namespace Thrift.Net.Tests.Compilation.Symbols
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class DocumentTests
    {
        [Fact]
        public void CSharpNamespace_AllScopeProvided_ReturnsNamespace()
        {
            // Arrange
            var document = new DocumentBuilder()
                .AddNamespace(builder => builder
                    .SetScope(Namespace.AllNamespacesScope)
                    .SetNamespaceName("Thrift.Net.Tests"))
                .Build();

            // Act
            var csharpNamespace = document.CSharpNamespace;

            // Assert
            Assert.Equal("Thrift.Net.Tests", csharpNamespace);
        }

        [Fact]
        public void CSharpNamespace_NoNamespacesProvided_ReturnsNull()
        {
            // Arrange
            var document = new DocumentBuilder()
                .Build();

            // Act
            var csharpNamespace = document.CSharpNamespace;

            // Assert
            Assert.Null(csharpNamespace);
        }

        [Theory]
        [InlineData("csharp")]
        [InlineData("netcore")]
        [InlineData("netstd")]
        public void CSharpNamespace_CSharpScopesTakePrecedenceOverAllScope(
            string csharpScope)
        {
            // Arrange
            var document = new DocumentBuilder()
                .AddNamespace(builder => builder
                    .SetScope(csharpScope)
                    .SetNamespaceName("CSharpNamespace"))
                .AddNamespace(builder => builder
                    .SetScope(Namespace.AllNamespacesScope)
                    .SetNamespaceName("AllNamespace"))
                .Build();

            // Act
            var csharpNamespace = document.CSharpNamespace;

            // Assert
            Assert.Equal("CSharpNamespace", csharpNamespace);
        }

        [Fact]
        public void CSharpNamespace_MultipleValidSpecified_LastWins()
        {
            // Arrange
            var document = new DocumentBuilder()
                .AddNamespace(builder => builder
                    .SetScope("netstd")
                    .SetNamespaceName("NetStd"))
                .AddNamespace(builder => builder
                    .SetScope("csharp")
                    .SetNamespaceName("CSharp"))
                .Build();

            // Act
            var csharpNamespace = document.CSharpNamespace;

            // Assert
            Assert.Equal("CSharp", csharpNamespace);
        }
    }
}