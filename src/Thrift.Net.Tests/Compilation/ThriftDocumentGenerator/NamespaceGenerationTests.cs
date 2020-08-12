namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Thrift.Net.Compilation.Model;
    using Xunit;
    using ThriftDocumentGenerator = Thrift.Net.Compilation.ThriftDocumentGenerator;

    public class NamespaceGenerationTests : ThriftDocumentGeneratorTests
    {
        [Fact]
        public void Generate_NamespaceSupplied_SetsCorrectNamespace()
        {
            // Arrange
            var generator = new ThriftDocumentGenerator();
            var document = new ThriftDocument("Thrift.Net.Tests", new List<EnumDefinition>());

            // Act
            var output = generator.Generate(document);

            // Assert
            var root = ParseCSharp(output);
            var namespaceDeclaration = root.Members.First() as NamespaceDeclarationSyntax;
            Assert.Equal("Thrift.Net.Tests", namespaceDeclaration.Name.ToString());
        }
    }
}