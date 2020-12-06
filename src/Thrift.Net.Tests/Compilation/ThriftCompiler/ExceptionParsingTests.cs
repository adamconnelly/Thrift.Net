namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using System.Linq;
    using Thrift.Net.Tests.Extensions;
    using Xunit;

    using ThriftCompiler = Thrift.Net.Compilation.ThriftCompiler;

    public class ExceptionParsingTests
    {
        private readonly ThriftCompiler compiler = new ThriftCompiler();

        [Fact]
        public void Compile_DocumentContainsException_AddsExceptionToModel()
        {
            // Arrange
            var input = "exception NotFoundException {}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Exceptions,
                item => Assert.Equal("NotFoundException", item.Name));
        }

        [Fact]
        public void Compile_DocumentContainsMultipleExceptions_AddsAllToModel()
        {
            // Arrange
            var input =
@"exception NotFoundException {}
exception NotEnabledException {}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Collection(
                result.Document.Exceptions,
                item => Assert.Equal("NotFoundException", item.Name),
                item => Assert.Equal("NotEnabledException", item.Name));
        }

        [Fact]
        public void Compile_ExceptionContainsFields_AddsFieldsToException()
        {
            // Arrange
            var input =
@"exception NotFoundException {
    i32 Id
    string Name
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            var definition = result.Document.Exceptions.First();

            Assert.Collection(
                definition.Fields,
                item => Assert.Equal("Id", item.Name),
                item => Assert.Equal("Name", item.Name));
        }

        [Fact]
        public void Compile_ExceptionContainsFields_SetsFieldIdsCorrectly()
        {
            // Arrange
            var input =
@"exception NotFoundException {
    i32 Id
    1: string Username
    2: string Timestamp
    string ErrorId
    string Type
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            var definition = result.Document.Exceptions.First();

            Assert.Collection(
                definition.Fields,
                item => Assert.Equal(-1, item.FieldId),
                item => Assert.Equal(1, item.FieldId),
                item => Assert.Equal(2, item.FieldId),
                item => Assert.Equal(-2, item.FieldId),
                item => Assert.Equal(-3, item.FieldId));
        }

        [Fact]
        public void Compile_ExceptionUsesCommaFieldSeparators_ParsesCorrectly()
        {
            // Arrange
            var input =
@"exception NotFoundException {
    i32 Id,
    string Username
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Document.Exceptions.Single().Fields.Count());
        }

        [Fact]
        public void Compile_ExceptionUsesSemicolonFieldSeparators_ParsesCorrectly()
        {
            // Arrange
            var input =
@"exception NotFoundException {
    i32 Id;
    string Username
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Document.Exceptions.Single().Fields.Count());
        }

        [Fact]
        public void Compile_MultipleExceptions_AssignsFieldsToCorrectException()
        {
            // Arrange
            var input =
@"exception NotFoundException {
    1: i32 Id
    2: string Username
}

exception OperationFailedException {
    1: string Operation
    2: string Timestamp
    3: string Details
}";

            // Act
            var result = this.compiler.Compile(input.ToStream());

            // Assert
            Assert.Equal(2, result.Document.Exceptions.Count);
            Assert.Equal(2, result.Document.Exceptions.ElementAt(0).Fields.Count);
            Assert.Equal(3, result.Document.Exceptions.ElementAt(1).Fields.Count);
        }
    }
}