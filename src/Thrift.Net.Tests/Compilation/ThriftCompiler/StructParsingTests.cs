namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using System.Linq;
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
    }
}