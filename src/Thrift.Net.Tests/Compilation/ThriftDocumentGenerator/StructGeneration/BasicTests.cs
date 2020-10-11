namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator.StructGeneration
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Extensions;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class BasicTests : ThriftDocumentGeneratorTests
    {
        public static IEnumerable<object[]> GetBaseTypes()
        {
            var field = Substitute.For<IField>();

            return BaseType.Names.Select(name => new[]
            {
                BaseType.Resolve(
                    ParserInput
                        .FromString(name)
                        .ParseInput(parser => parser.baseType()),
                    field),
            });
        }

        [Fact]
        public void Generate_StructsProvided_GeneratesStructs()
        {
            // Arrange
            var input =
@"namespace csharp Thrift.Net.Examples
struct User {}
struct Permission {}";
            var result = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(result.Document);

            // Assert
            var (root, _, _) = ParseCSharp(output);
            var namespaceDeclaration = root.GetCompilationUnitRoot().Members.First() as NamespaceDeclarationSyntax;
            var classes = namespaceDeclaration.Members.OfType<ClassDeclarationSyntax>();

            Assert.Collection(
                classes,
                userClass => Assert.Equal("User", userClass.Identifier.Text),
                permissionClass => Assert.Equal("Permission", permissionClass.Identifier.Text));
        }

        [Fact]
        public void Generate_StructHasFields_GeneratesFields()
        {
            // Arrange
            var input =
@"namespace csharp Thrift.Net.Examples
struct User {
    bool Field1
    bool Field2
}";
            var result = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(result.Document);

            // Assert
            var (root, _, _) = ParseCSharp(output);
            var namespaceDeclaration = root.GetCompilationUnitRoot().Members.First() as NamespaceDeclarationSyntax;
            var userClass = namespaceDeclaration.Members
                .OfType<ClassDeclarationSyntax>()
                .First();
            var fieldProperties = userClass.Members
                .OfType<PropertyDeclarationSyntax>()
                .Where(property => property.Identifier.Text.StartsWith("Field"));

            Assert.Collection(
                fieldProperties,
                field1 => Assert.Equal("Field1", field1.Identifier.Text),
                field2 => Assert.Equal("Field2", field2.Identifier.Text));
        }

        [Theory]
        [MemberData(nameof(GetBaseTypes))]
        public void Generate_StructHasFields_GeneratesPropertiesWithCorrectTypes(
            BaseType type)
        {
            // Arrange
            var input =
@$"namespace csharp Thrift.Net.Examples
struct User {{
    {type.Name} OptionalField
    required {type.Name} RequiredField
}}";
            var result = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(result.Document);

            // Assert
            var (root, _, _) = ParseCSharp(output);
            var namespaceDeclaration = root.GetCompilationUnitRoot().Members.First() as NamespaceDeclarationSyntax;
            var userClass = namespaceDeclaration.Members
                .OfType<ClassDeclarationSyntax>()
                .First();

            var optionalProperty = userClass.Members
                .OfType<PropertyDeclarationSyntax>()
                .Single(property => property.Identifier.Text == "OptionalField");
            var requiredProperty = userClass.Members
                .OfType<PropertyDeclarationSyntax>()
                .Single(property => property.Identifier.Text == "RequiredField");

            string optionalTypeName = GetPropertyTypeName(optionalProperty.Type);
            string requiredTypeName = GetPropertyTypeName(requiredProperty.Type);

            Assert.Equal(type.CSharpOptionalTypeName, optionalTypeName);
            Assert.Equal(type.CSharpRequiredTypeName, requiredTypeName);
        }

        [Fact]
        public void Generate_StructHasEnumField_GeneratesPropertyWithCorrectType()
        {
            // Arrange
            var input =
@"namespace csharp Thrift.Net.Examples

enum UserType {}

struct User {
    UserType OptionalField
    required UserType RequiredField
}";
            var result = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(result.Document);

            // Assert
            var (root, _, _) = ParseCSharp(output);
            var namespaceDeclaration = root.GetCompilationUnitRoot().Members.First() as NamespaceDeclarationSyntax;
            var userClass = namespaceDeclaration.Members
                .OfType<ClassDeclarationSyntax>()
                .First();

            var optionalProperty = userClass.Members
                .OfType<PropertyDeclarationSyntax>()
                .Single(property => property.Identifier.Text == "OptionalField");
            var requiredProperty = userClass.Members
                .OfType<PropertyDeclarationSyntax>()
                .Single(property => property.Identifier.Text == "RequiredField");

            string optionalTypeName = GetPropertyTypeName(optionalProperty.Type);
            string requiredTypeName = GetPropertyTypeName(requiredProperty.Type);

            Assert.Equal("Thrift.Net.Examples.UserType?", optionalTypeName);
            Assert.Equal("Thrift.Net.Examples.UserType", requiredTypeName);
        }

        [Fact]
        public void Generate_FieldsSupplied_GeneratesFieldIdConstants()
        {
            // Arrange
            var input =
@"namespace csharp Thrift.Net.Examples
struct User {
    bool Field1
    bool Field2
    1: bool Field3
    5: bool field4
}";
            var result = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(result.Document);

            // Assert
            var (tree, compilation, semanticModel) = ParseCSharp(output);
            var compilationUnit = tree.GetCompilationUnitRoot();
            var namespaceDeclaration = compilationUnit.Members.First() as NamespaceDeclarationSyntax;
            var classDeclaration = namespaceDeclaration.Members.First() as ClassDeclarationSyntax;

            var fieldIdsMember = classDeclaration.Members
                .OfType<ClassDeclarationSyntax>()
                .Where(declaration => declaration.Identifier.Text == "FieldIds")
                .Single();

            var fieldIds = fieldIdsMember.Members.OfType<FieldDeclarationSyntax>();

            Assert.Collection(
                fieldIds,
                field1 => ValidateFieldIdConstant(field1, -1, semanticModel),
                field2 => ValidateFieldIdConstant(field2, -2, semanticModel),
                field3 => ValidateFieldIdConstant(field3, 1, semanticModel),
                field4 => ValidateFieldIdConstant(field4, 5, semanticModel));
        }

        private static void ValidateFieldIdConstant(
            FieldDeclarationSyntax fieldSyntax,
            int expectedValue,
            SemanticModel semanticModel)
        {
            var variable = fieldSyntax.Declaration.Variables.Single();
            var fieldSymbol = semanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;
            var initializerExpression = variable.Initializer.Value;
            var value = semanticModel.GetConstantValue(initializerExpression);

            // If the value is negative (e.g. `-1`), `GetConstantValue()` won't
            // be able to calculate it. In that case we need to get the operand
            // of the expression, and multiply it by -1 to get the value.
            if (initializerExpression is PrefixUnaryExpressionSyntax prefixSyntax)
            {
                var operandValue = semanticModel.GetConstantValue(prefixSyntax.Operand);
                value = ((int)operandValue.Value) * -1;
            }

            Assert.True(fieldSymbol.IsConst);
            Assert.Equal(Accessibility.Public, fieldSymbol.DeclaredAccessibility);
            Assert.Equal(expectedValue, value);
        }

        private static string GetPropertyTypeName(TypeSyntax propertyType)
        {
            string typeName;

            if (propertyType is NullableTypeSyntax nullableType)
            {
                return GetPropertyTypeName(nullableType.ElementType) + "?";
            }
            else if (propertyType is ArrayTypeSyntax arrayType)
            {
                var typeNode = arrayType.ElementType as PredefinedTypeSyntax;
                typeName = typeNode.Keyword.Text + "[]";
            }
            else if (propertyType is QualifiedNameSyntax qualifiedName)
            {
                typeName = qualifiedName.ToString();
            }
            else
            {
                var typeNode = propertyType as PredefinedTypeSyntax;
                typeName = typeNode.Keyword.Text;
            }

            return typeName;
        }
    }
}
