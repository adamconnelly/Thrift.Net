namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;
    using ThriftDocumentGenerator = Thrift.Net.Compilation.ThriftDocumentGenerator;

    public class EnumGenerationTests : ThriftDocumentGeneratorTests
    {
        [Fact]
        public void Generate_EnumProvided_SetsCorrectName()
        {
            // Arrange
            var model = CreateDocument();

            // Act
            var output = this.Generator.Generate(model);

            // Assert
            var userTypeEnum = GetEnum(output);

            Assert.Equal("UserType", userTypeEnum.Identifier.ValueText);
        }

        [Fact]
        public void Generate_EnumProvided_AddsMembers()
        {
            // Arrange
            var model = CreateDocument();

            // Act
            var output = this.Generator.Generate(model);

            // Assert
            var userTypeEnum = GetEnum(output);
            var userMember = userTypeEnum.Members.ElementAt(0) as EnumMemberDeclarationSyntax;
            var adminMember = userTypeEnum.Members.ElementAt(1) as EnumMemberDeclarationSyntax;

            Assert.Equal("User", userMember.Identifier.ValueText);
            Assert.Equal("Administrator", adminMember.Identifier.ValueText);
        }

        [Fact]
        public void Generate_EnumProvided_SetsMemberValues()
        {
            // Arrange
            var model = CreateDocument();

            // Act
            var output = this.Generator.Generate(model);

            // Assert
            var userTypeEnum = GetEnum(output);
            var userMember = userTypeEnum.Members.ElementAt(0) as EnumMemberDeclarationSyntax;
            var adminMember = userTypeEnum.Members.ElementAt(1) as EnumMemberDeclarationSyntax;

            Assert.Equal("2", userMember.EqualsValue.Value.ToString());
            Assert.Equal("5", adminMember.EqualsValue.Value.ToString());
        }

        private static EnumDeclarationSyntax GetEnum(string output)
        {
            var (root, _, _) = ParseCSharp(output);
            var namespaceDeclaration = root.GetCompilationUnitRoot().Members.First() as NamespaceDeclarationSyntax;

            return namespaceDeclaration.Members.First() as EnumDeclarationSyntax;
        }

        private static Document CreateDocument()
        {
            return new DocumentBuilder()
                .AddNamespace(builder => builder
                    .SetScope("csharp")
                    .SetNamespaceName("Thrift.Net.Tests"))
                .AddEnum(builder => builder
                    .SetName("UserType")
                    .AddMember(builder => builder.SetName("User").SetValue(2))
                    .AddMember(builder => builder.SetName("Administrator").SetValue(5)))
                .Build();
        }
    }
}