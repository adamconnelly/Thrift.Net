namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class StructGenerationTests : ThriftDocumentGeneratorTests
    {
        public static IEnumerable<object[]> GetBaseTypes()
        {
            return FieldType.BaseTypes.Select(baseType => new[] { baseType });
        }

        [Fact]
        public void Generate_StructsProvided_GeneratesStructs()
        {
            // Arrange
            var document = new ThriftDocument(
                "Thrift.Net.Examples",
                new List<Enum>(),
                new List<Struct>
                {
                    new StructBuilder().SetName("User").Build(),
                    new StructBuilder().SetName("Permission").Build(),
                });

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var root = ParseCSharp(output);
            var namespaceDeclaration = root.Members.First() as NamespaceDeclarationSyntax;
            var classes = namespaceDeclaration.Members.OfType<ClassDeclarationSyntax>();

            Assert.Collection(
                classes,
                userClass => Assert.Equal("User", userClass.Identifier.Text),
                permissionClass => Assert.Equal("Permission", permissionClass.Identifier.Text));
        }

        [Fact]
        public void Generate_StructsHasFields_GeneratesFields()
        {
            // Arrange
            var document = new ThriftDocument(
                "Thrift.Net.Examples",
                new List<Enum>(),
                new List<Struct>
                {
                    new StructBuilder()
                        .SetName("User")
                        .AddField(builder => builder
                            .SetType(FieldType.Bool)
                            .SetName("Field1"))
                        .AddField(builder => builder
                            .SetType(FieldType.Bool)
                            .SetName("Field2"))
                        .Build(),
                });

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var root = ParseCSharp(output);
            var namespaceDeclaration = root.Members.First() as NamespaceDeclarationSyntax;
            var userClass = namespaceDeclaration.Members
                .OfType<ClassDeclarationSyntax>()
                .First();

            Assert.Collection(
                userClass.Members.OfType<PropertyDeclarationSyntax>(),
                field1 => Assert.Equal("Field1", field1.Identifier.Text),
                field2 => Assert.Equal("Field2", field2.Identifier.Text));
        }

        [Theory]
        [MemberData(nameof(GetBaseTypes))]
        public void Generate_StructsHasFields_GeneratesPropertiesWithCorrectTypes(
            FieldType type)
        {
            // Arrange
            var document = new ThriftDocument(
                "Thrift.Net.Examples",
                new List<Enum>(),
                new List<Struct>
                {
                    new StructBuilder()
                        .SetName("User")
                        .AddField(builder => builder
                            .SetName("Field")
                            .SetType(type))
                        .Build(),
                });

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var root = ParseCSharp(output);
            var namespaceDeclaration = root.Members.First() as NamespaceDeclarationSyntax;
            var userClass = namespaceDeclaration.Members
                .OfType<ClassDeclarationSyntax>()
                .First();

            var property = userClass.Members
                .OfType<PropertyDeclarationSyntax>()
                .Single();

            string typeName;

            if (property.Type is NullableTypeSyntax nullableType)
            {
                var typeNode = nullableType.ElementType as PredefinedTypeSyntax;
                typeName = typeNode.Keyword.Text + "?";
            }
            else if (property.Type is ArrayTypeSyntax arrayType)
            {
                var typeNode = arrayType.ElementType as PredefinedTypeSyntax;
                typeName = typeNode.Keyword.Text + "[]";
            }
            else
            {
                var typeNode = property.Type as PredefinedTypeSyntax;
                typeName = typeNode.Keyword.Text;
            }

            Assert.Equal(type.CSharpTypeName, typeName);
        }
    }
}