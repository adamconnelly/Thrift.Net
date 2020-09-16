namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator.StructGeneration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Xunit;

    public class IsSetTests : ThriftDocumentGeneratorTests
    {
        [Fact]
        public void HasFieldsWithDefaultRequiredness_GeneratesIsSetStruct()
        {
            // Arrange
            var document = new DocumentBuilder()
                .AddNamespace(builder => builder
                    .SetScope("csharp")
                    .SetNamespaceName("Thrift.Net.Examples"))
                .AddStruct(builder => builder
                    .SetName("User")
                    .AddField(builder => builder
                        .SetType(FieldType.Bool)
                        .SetName("Field1"))
                    .AddField(builder => builder
                        .SetType(FieldType.Bool)
                        .SetName("Field2")))
                .Build();

            // Act
            var output = this.Generator.Generate(document);

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
            var document = new DocumentBuilder()
                .AddNamespace(builder => builder
                    .SetScope("csharp")
                    .SetNamespaceName("Thrift.Net.Examples"))
                .AddStruct(builder => builder.SetName("User"))
                .Build();

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var (isSetStruct, isSetProperty, _) = GetIsSetInformation(output);

            Assert.Null(isSetStruct);
            Assert.Null(isSetProperty);
        }

        [Fact]
        public void OnlyHasRequiredFields_DoesNotGenerateIsSetStruct()
        {
            // Arrange
            var document = new DocumentBuilder()
                .AddNamespace(builder => builder
                    .SetScope("csharp")
                    .SetNamespaceName("Thrift.Net.Examples"))
                .AddStruct(builder => builder
                    .SetName("User")
                    .AddField(builder => builder
                        .SetType(FieldType.Bool)
                        .SetName("Field1")
                        .SetRequiredness(FieldRequiredness.Required))
                    .AddField(builder => builder
                        .SetType(FieldType.Bool)
                        .SetName("Field2")
                        .SetRequiredness(FieldRequiredness.Required)))
                .Build();

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var (isSetStruct, isSetProperty, _) = GetIsSetInformation(output);

            Assert.Null(isSetStruct);
            Assert.Null(isSetProperty);
        }

        [Fact]
        public void OnlyHasOptionalFields_GeneratesIsSetStruct()
        {
            // Arrange
            var document = new DocumentBuilder()
                .AddNamespace(builder => builder
                    .SetScope("csharp")
                    .SetNamespaceName("Thrift.Net.Examples"))
                .AddStruct(builder => builder
                    .SetName("User")
                    .AddField(builder => builder
                        .SetName("Field1")
                        .SetType(FieldType.Bool)
                        .SetRequiredness(FieldRequiredness.Optional))
                    .AddField(builder => builder
                        .SetName("Field2")
                        .SetType(FieldType.Bool)
                        .SetRequiredness(FieldRequiredness.Optional)))
                .Build();

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var (isSetStruct, isSetProperty, _) = GetIsSetInformation(output);

            Assert.NotNull(isSetStruct);
            Assert.NotNull(isSetProperty);
        }

        [Fact]
        public async Task HasFields_DefaultsIsSetToFalseForEachField()
        {
            // Arrange
            var document = new DocumentBuilder()
                .AddStruct(builder => builder
                    .SetName("User")
                    .AddField(builder => builder
                        .SetName("Field1")
                        .SetFieldId(0)
                        .SetType(FieldType.Bool))
                    .AddField(builder => builder
                        .SetName("Field2")
                        .SetFieldId(1)
                        .SetType(FieldType.Bool)))
                .Build();

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var scriptContents =
@$"{output}

var user = new User();
return user.IsSet.Field1 == false &&
    user.IsSet.Field2 == false;";

            var script = CSharpScript.Create(scriptContents);
            script.Compile();

            var result = (bool)(await script.RunAsync()).ReturnValue;

            Assert.True(result);
        }

        [Fact]
        public async Task FieldSet_MarksIsSetTrueForField()
        {
            // Arrange
            var document = new DocumentBuilder()
                .AddStruct(builder => builder
                    .SetName("User")
                    .AddField(builder => builder
                        .SetName("Field1")
                        .SetFieldId(0)
                        .SetType(FieldType.Bool)))
                .Build();

            // Act
            var output = this.Generator.Generate(document);

            // Assert
            var scriptContents =
@$"{output}

var user = new User();
user.Field1 = true;
return user.IsSet.Field1;";

            var script = CSharpScript.Create(scriptContents);
            script.Compile();

            var result = (bool)(await script.RunAsync()).ReturnValue;

            Assert.True(result);
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
