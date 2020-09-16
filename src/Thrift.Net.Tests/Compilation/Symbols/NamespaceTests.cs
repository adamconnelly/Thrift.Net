namespace Thrift.Net.Tests.Compilation.Symbols
{
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class NamespaceTests
    {
        [Theory]
        [InlineData("*", true)]
        [InlineData("delphi", false)]
        [InlineData("csharp", true)]
        [InlineData("netcore", true)]
        [InlineData("netstd", true)]
        public void CanGenerate_ScopesProvided_ReturnsCorrectValue(
            string scope, bool expected)
        {
            // Arrange
            var ns = new NamespaceBuilder()
                .SetScope(scope)
                .Build();

            // Act
            var canGenerate = ns.CanGenerate;

            // Assert
            Assert.Equal(expected, canGenerate);
        }

        [Theory]
        [InlineData("*", true)]
        [InlineData("c_glib", true)]
        [InlineData("cpp", true)]
        [InlineData("csharp", true)]
        [InlineData("delphi", true)]
        [InlineData("go", true)]
        [InlineData("java", true)]
        [InlineData("js", true)]
        [InlineData("lua", true)]
        [InlineData("netcore", true)]
        [InlineData("perl", true)]
        [InlineData("php", true)]
        [InlineData("py", true)]
        [InlineData("py.twisted", true)]
        [InlineData("rb", true)]
        [InlineData("st", true)]
        [InlineData("xsd", true)]
        [InlineData("netstd", true)]
        [InlineData("fortran", false)]
        public void IsKnownScope_ScopesProvided_ReturnsCorrectValue(
            string scope, bool expected)
        {
            // Arrange
            var ns = new NamespaceBuilder()
                .SetScope(scope)
                .Build();

            // Act
            var canGenerate = ns.HasKnownScope;

            // Assert
            Assert.Equal(expected, canGenerate);
        }

        [Theory]
        [InlineData("*", true)]
        [InlineData("csharp", false)]
        [InlineData("fortran", false)]
        public void AppliesToAllTargets_ScopesProvided_ReturnsCorrectValue(
            string scope, bool expected)
        {
            // Arrange
            var ns = new NamespaceBuilder()
                .SetScope(scope)
                .Build();

            // Act
            var canGenerate = ns.AppliesToAllTargets;

            // Assert
            Assert.Equal(expected, canGenerate);
        }
    }
}