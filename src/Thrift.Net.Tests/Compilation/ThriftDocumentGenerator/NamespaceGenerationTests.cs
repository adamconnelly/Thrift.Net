namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Extensions;
    using Xunit;
    using ThriftDocumentGenerator = Thrift.Net.Compilation.ThriftDocumentGenerator;

    public class NamespaceGenerationTests : ThriftDocumentGeneratorTests
    {
        [Fact]
        public void Generate_NamespaceSupplied_SetsCorrectNamespace()
        {
            // Arrange
            var input = "namespace csharp Thrift.Net.Tests";
            var compilationResult = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(this.ThriftFile, compilationResult.Document);

            // Assert
            var (root, _, _) = ParseCSharp(output);
            var namespaceDeclaration = root.GetCompilationUnitRoot().Members.First() as NamespaceDeclarationSyntax;
            Assert.Equal("Thrift.Net.Tests", namespaceDeclaration.Name.ToString());
        }

        [Fact]
        public void Generate_NamespaceNotSupplied_DoesNotGenerateNamespace()
        {
            // Arrange;
            var input = "enum UserType {}";
            var compilationResult = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(this.ThriftFile, compilationResult.Document);

            // Assert
            var (root, _, _) = ParseCSharp(output);
            var enumDeclaration = Assert.IsAssignableFrom<EnumDeclarationSyntax>(
                root.GetCompilationUnitRoot().Members.First());
            Assert.Equal("UserType", enumDeclaration.Identifier.Text);
        }
    }
}
