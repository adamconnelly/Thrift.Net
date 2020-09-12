namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Thrift.Net.Compilation.Symbols;
    using Xunit;
    using ThriftDocumentGenerator = Thrift.Net.Compilation.ThriftDocumentGenerator;

    public class EnumGenerationTests : ThriftDocumentGeneratorTests
    {
        private readonly ThriftDocumentGenerator generator = new ThriftDocumentGenerator();

        [Fact]
        public void Generate_EnumProvided_SetsCorrectName()
        {
            // Arrange
            var model = CreateDocument();

            // Act
            var output = this.generator.Generate(model);

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
            var output = this.generator.Generate(model);

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
            var output = this.generator.Generate(model);

            // Assert
            var userTypeEnum = GetEnum(output);
            var userMember = userTypeEnum.Members.ElementAt(0) as EnumMemberDeclarationSyntax;
            var adminMember = userTypeEnum.Members.ElementAt(1) as EnumMemberDeclarationSyntax;

            Assert.Equal("2", userMember.EqualsValue.Value.ToString());
            Assert.Equal("5", adminMember.EqualsValue.Value.ToString());
        }

        private static EnumDeclarationSyntax GetEnum(string output)
        {
            var root = ParseCSharp(output);
            var namespaceDeclaration = root.Members.First() as NamespaceDeclarationSyntax;

            return namespaceDeclaration.Members.First() as EnumDeclarationSyntax;
        }

        private static ThriftDocument CreateDocument()
        {
            return new ThriftDocument(
                "Thrift.Net.Tests",
                new List<Enum>
                {
                    new Enum("UserType", new List<EnumMember>
                    {
                        new EnumMember("User", 2),
                        new EnumMember("Administrator", 5),
                    }),
                },
                new List<Struct>());
        }
    }
}