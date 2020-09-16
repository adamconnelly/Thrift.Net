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
            var document = new DocumentBuilder()
                .AddNamespace(builder => builder
                    .SetScope("csharp")
                    .SetNamespaceName("Thrift.Net.Tests"))
                .Build();

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
            var document = new DocumentBuilder()
                .AddEnum(builder => builder.SetName("UserType"))
                .Build();

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