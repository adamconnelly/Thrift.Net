namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator.StructGeneration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Scripting;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Extensions;
    using Thrift.Protocol;
    using Xunit;

    public class IsSetTests : ThriftDocumentGeneratorTests
    {
        [Fact]
        public void HasFieldsWithDefaultRequiredness_GeneratesIsSetStruct()
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
            var (isSetStruct, isSetProperty, isSetSymbol) = GetIsSetInformation(output);

            Assert.NotNull(isSetStruct);
            Assert.NotNull(isSetProperty);
            Assert.Equal(isSetStruct.Identifier.Text, isSetSymbol.Type.Name);
        }

        [Fact]
        public void HasNoFields_DoesNotGenerateIsSetStruct()
        {
            // Arrange
            var input =
@"namespace csharp Thrift.Net.Examples
struct User {}";
            var result = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(result.Document);

            // Assert
            var (isSetStruct, isSetProperty, _) = GetIsSetInformation(output);

            Assert.Null(isSetStruct);
            Assert.Null(isSetProperty);
        }

        [Fact]
        public void OnlyHasRequiredFields_DoesNotGenerateIsSetStruct()
        {
            // Arrange
            var input =
@"namespace csharp Thrift.Net.Examples
struct User {
    required bool Field1
    required bool Field2
}";
            var result = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(result.Document);

            // Assert
            var (isSetStruct, isSetProperty, _) = GetIsSetInformation(output);

            Assert.Null(isSetStruct);
            Assert.Null(isSetProperty);
        }

        [Fact]
        public void OnlyHasOptionalFields_GeneratesIsSetStruct()
        {
            // Arrange
            var input =
@"namespace csharp Thrift.Net.Examples
struct User {
    optional bool Field1
    optional bool Field2
}";
            var result = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(result.Document);

            // Assert
            var (isSetStruct, isSetProperty, _) = GetIsSetInformation(output);

            Assert.NotNull(isSetStruct);
            Assert.NotNull(isSetProperty);
        }

        [Fact]
        public async Task HasFields_DefaultsIsSetToFalseForEachField()
        {
            // Arrange
            var input =
@"struct User {
    0: bool Field1
    1: bool Field2
}";
            var compilationResult = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(compilationResult.Document);

            // Assert
            var scriptContents =
@$"{output}

var user = new User();
return user.IsSet.Field1 == false &&
    user.IsSet.Field2 == false;";

            var script = CSharpScript.Create(
                scriptContents, ScriptOptions.Default.WithReferences(typeof(TBase).Assembly));
            script.Compile();

            var result = (bool)(await script.RunAsync()).ReturnValue;

            Assert.True(result);
        }

        [Fact]
        public async Task FieldSet_MarksIsSetTrueForField()
        {
            // Arrange
            var input =
@"struct User {
    0: bool Field1
}";
            var compilationResult = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(compilationResult.Document);

            // Assert
            var scriptContents =
@$"{output}

var user = new User();
user.Field1 = true;
return user.IsSet.Field1;";

            var script = CSharpScript.Create(
                scriptContents, ScriptOptions.Default.WithReferences(typeof(TBase).Assembly));
            script.Compile();

            var result = (bool)(await script.RunAsync()).ReturnValue;

            Assert.True(result);
        }

        [Fact]
        public async Task FieldSetToNull_MarksIsSetFalseForField()
        {
            // Arrange
            var input =
@"struct User {
    0: bool Field1
}";
            var compilationResult = this.Compiler.Compile(input.ToStream());

            // Act
            var output = this.Generator.Generate(compilationResult.Document);

            // Assert
            var scriptContents =
@$"{output}

var user = new User();
user.Field1 = null;
return user.IsSet.Field1;";

            var script = CSharpScript.Create(
                scriptContents, ScriptOptions.Default.WithReferences(typeof(TBase).Assembly));
            script.Compile();

            var result = (bool)(await script.RunAsync()).ReturnValue;

            Assert.False(result);
        }

        private static (StructDeclarationSyntax, PropertyDeclarationSyntax, IPropertySymbol)
            GetIsSetInformation(string output)
        {
            var (root, _, semanticModel) = ParseCSharp(output);
            var namespaceDeclaration = root.GetCompilationUnitRoot().Members.First() as NamespaceDeclarationSyntax;
            var userClass = namespaceDeclaration.Members
                .OfType<ClassDeclarationSyntax>()
                .First();
            var isSetStruct = userClass.Members
                .OfType<StructDeclarationSyntax>()
                .Where(definition => definition.Identifier.Text == "IsSetInfo")
                .FirstOrDefault();
            var isSetProperty = userClass.Members
                .OfType<PropertyDeclarationSyntax>()
                .Where(property => property.Identifier.Text == "IsSet")
                .FirstOrDefault();
            IPropertySymbol isSetSymbol = null;
            if (isSetProperty != null)
            {
                isSetSymbol = semanticModel.GetDeclaredSymbol(isSetProperty);
            }

            return (isSetStruct, isSetProperty, isSetSymbol);
        }
    }
}
