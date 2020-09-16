namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;
    using ThriftDocumentGenerator = Thrift.Net.Compilation.ThriftDocumentGenerator;

    public class NamespaceGenerationTests : ThriftDocumentGeneratorTests
    {
        [Fact]
        public void Generate_NamespaceSupplied_SetsCorrectNamespace()
        {
            // Arrange
            var document = new Document(
                "Thrift.Net.Tests", new List<Enum>(), new List<Struct>());

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var (root, _, _) = ParseCSharp(output);
            var namespaceDeclaration = root.GetCompilationUnitRoot().Members.First() as NamespaceDeclarationSyntax;
            Assert.Equal("Thrift.Net.Tests", namespaceDeclaration.Name.ToString());
        }

        [Fact]
        public void Generate_NamespaceNotSupplied_DoesNotGenerateNamespace()
        {
            // Arrange
            var document = new Document(
                null,
                new List<Enum>
                {
                    new EnumBuilder().SetName("UserType").Build(),
                },
                new List<Struct>());

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var (root, _, _) = ParseCSharp(output);
            var enumDeclaration = Assert.IsAssignableFrom<EnumDeclarationSyntax>(
                root.GetCompilationUnitRoot().Members.First());
            Assert.Equal("UserType", enumDeclaration.Identifier.Text);
        }
    }
}