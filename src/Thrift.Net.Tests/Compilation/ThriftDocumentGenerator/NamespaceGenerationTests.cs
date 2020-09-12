namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using System.Collections.Generic;
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
            var generator = new ThriftDocumentGenerator();
            var document = new ThriftDocument(
                "Thrift.Net.Tests", new List<Enum>(), new List<Struct>());

            // Act
            var output = generator.Generate(document);

            // Assert
            var root = ParseCSharp(output);
            var namespaceDeclaration = root.Members.First() as NamespaceDeclarationSyntax;
            Assert.Equal("Thrift.Net.Tests", namespaceDeclaration.Name.ToString());
        }

        [Fact]
        public void Generate_NamespaceNotSupplied_DoesNotGenerateNamespace()
        {
            // Arrange
            var generator = new ThriftDocumentGenerator();
            var document = new ThriftDocument(
                null,
                new List<Enum>
                {
                    new EnumBuilder().SetName("UserType").Build(),
                },
                new List<Struct>());

            // Act
            var output = generator.Generate(document);

            // Assert
            var root = ParseCSharp(output);
            var enumDeclaration = Assert.IsAssignableFrom<EnumDeclarationSyntax>(
                root.Members.First());
            Assert.Equal("UserType", enumDeclaration.Identifier.Text);
        }
    }
}