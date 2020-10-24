namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using System.Linq;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Extensions;
    using Xunit;

    using ThriftCompiler = Thrift.Net.Compilation.ThriftCompiler;

    public class StructParsingTests
    {
        private readonly ThriftCompiler compiler = new ThriftCompiler();

        [Fact]
        public void Compile_DocumentContainsStruct_AddsStructToModel()
        {
            // Arrange
            var input = "struct User {}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Structs,
                item => Assert.Equal("User", item.Name));
        }

        [Fact]
        public void Compile_DocumentContainsMultipleStructs_AddsAllToModel()
        {
            // Arrange
            var input =
@"struct User {}
struct Team {}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Structs,
                item => Assert.Equal("User", item.Name),
                item => Assert.Equal("Team", item.Name));
        }

        [Fact]
        public void Compile_StructContainsFields_AddsFieldsToStruct()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
    string Name
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            var definition = result.Document.Structs.First();

            Assert.Collection(
                definition.Fields,
                item => Assert.Equal("Id", item.Name),
                item => Assert.Equal("Name", item.Name));
        }

        [Fact]
        public void Compile_StructContainsFields_SetsFieldIdsCorrectly()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
    1: string Username
    2: string CreatedOn
    string Name
    string DeletedOn
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            var definition = result.Document.Structs.First();

            Assert.Collection(
                definition.Fields,
                item => Assert.Equal(-1, item.FieldId),
                item => Assert.Equal(1, item.FieldId),
                item => Assert.Equal(2, item.FieldId),
                item => Assert.Equal(-2, item.FieldId),
                item => Assert.Equal(-3, item.FieldId));
        }

        [Fact]
        public void Compile_StructUsesCommaFieldSeparators_ParsesCorrectly()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id,
    string Username
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Document.Structs.Single().Fields.Count());
        }

        [Fact]
        public void Compile_StructUsesSemicolonFieldSeparators_ParsesCorrectly()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id;
    string Username
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Document.Structs.Single().Fields.Count());
        }

        [Fact]
        public void Compile_MultipleStructs_AssignsFieldsToCorrectStruct()
        {
            // Arrange
            var input =
@"struct User {
    1: i32 Id
    2: string Username
}

struct Address {
    1: string Line1
    2: string Line2
    3: string Town
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Equal(2, result.Document.Structs.Count);
            Assert.Equal(2, result.Document.Structs.ElementAt(0).Fields.Count);
            Assert.Equal(3, result.Document.Structs.ElementAt(1).Fields.Count);
        }

        [Fact]
        public void Compile_StructReferencesEnum_ResolvesEnum()
        {
            // Arrange
            var input =
@"enum UserType {
    User
    Administrator
}

struct User {
    1: i32 Id
    2: string Username
    3: UserType Type
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            var field = result.Document.Structs.Single().Fields.FirstOrDefault(
                f => f.Name == "Type");

            Assert.Equal("UserType", field.Type.Name);
        }

        [Fact]
        public void Compile_StructHasList_ParsesList()
        {
            // Arrange
            var input =
@"struct User {
    1: list<string> Emails
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            var field = result.Document.Structs.Single().Fields.FirstOrDefault(
                f => f.Name == "Emails");

            var list = Assert.IsAssignableFrom<IListType>(field.Type);
            Assert.Equal(BaseType.String, list.ElementType.Name);
        }

        [Fact]
        public void Compile_StructHasSet_ParsesSet()
        {
            // Arrange
            var input =
@"struct User {
    1: set<string> Emails
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            var field = result.Document.Structs.Single().Fields.FirstOrDefault(
                f => f.Name == "Emails");

            var list = Assert.IsAssignableFrom<ISetType>(field.Type);
            Assert.Equal(BaseType.String, list.ElementType.Name);
        }
    }
}