namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using System.Linq;
    using Thrift.Net.Tests.Extensions;
    using Xunit;

    using ThriftCompiler = Thrift.Net.Compilation.ThriftCompiler;

    public class UnionParsingTests
    {
        private readonly ThriftCompiler compiler = new ThriftCompiler();

        [Fact]
        public void Compile_DocumentContainsStruct_AddsStructToModel()
        {
            // Arrange
            var input = "union User {}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Unions,
                item => Assert.Equal("User", item.Name));
        }

#pragma warning disable SA1005, SA1515
        [Fact]
        public void Compile_DocumentContainsMultipleUnions_AddsAllToModel()
        {
            // Arrange
            var input =
@"union User {}
union Team {}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Unions,
                item => Assert.Equal("User", item.Name),
                item => Assert.Equal("Team", item.Name));
        }

        [Fact]
        public void Compile_UnionContainsFields_AddsFieldsToUnion()
        {
            // Arrange
            var input =
@"union User {
    i32 Id
    string Name
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            var definition = result.Document.Unions.First();

            Assert.Collection(
                definition.Fields,
                item => Assert.Equal("Id", item.Name),
                item => Assert.Equal("Name", item.Name));
        }

        [Fact]
        public void Compile_UnionContainsFields_SetsFieldIdsCorrectly()
        {
            // Arrange
            var input =
@"union User {
    i32 Id
    1: string Username
    2: string CreatedOn
    string Name
    string DeletedOn
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            var definition = result.Document.Unions.First();

            Assert.Collection(
                definition.Fields,
                item => Assert.Equal(-1, item.FieldId),
                item => Assert.Equal(1, item.FieldId),
                item => Assert.Equal(2, item.FieldId),
                item => Assert.Equal(-2, item.FieldId),
                item => Assert.Equal(-3, item.FieldId));
        }

        [Fact]
        public void Compile_UnionUsesCommaFieldSeparators_ParsesCorrectly()
        {
            // Arrange
            var input =
@"union User {
    i32 Id,
    string Username
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Document.Unions.Single().Fields.Count());
        }

        [Fact]
        public void Compile_UnionUsesSemicolonFieldSeparators_ParsesCorrectly()
        {
            // Arrange
            var input =
@"union User {
    i32 Id;
    string Username
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Document.Unions.Single().Fields.Count());
        }

        [Fact]
        public void Compile_MultipleUnions_AssignsFieldsToCorrectUnion()
        {
            // Arrange
            var input =
@"union User {
    1: i32 Id
    2: string Username
}

union Address {
    1: string Line1
    2: string Line2
    3: string Town
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Equal(2, result.Document.Unions.Count);
            Assert.Equal(2, result.Document.Unions.ElementAt(0).Fields.Count);
            Assert.Equal(3, result.Document.Unions.ElementAt(1).Fields.Count);
        }
    }
}