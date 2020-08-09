namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using System.Linq;
    using Thrift.Net.Compilation;
    using Thrift.Net.Tests.Extensions;
    using Xunit;
    using ThriftCompiler = Thrift.Net.Compilation.ThriftCompiler;

    public class EnumErrorTests
    {
        [Fact]
        public void Compile_EnumNameMissing_ReportsError()
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var input = @"enum {}";

            // Act
            var result = compiler.Compile(input.ToStream());

            // Assert
            var error = result.Messages.First();
            Assert.Equal(CompilerMessageId.EnumMustHaveAName, error.MessageId);
            Assert.Equal(CompilerMessageType.Error, error.MessageType);
            Assert.Equal(1, error.LineNumber);
            Assert.Equal(1, error.StartPosition);
            Assert.Equal(4, error.EndPosition);
        }

        [Fact]
        public void Compile_EnumMemberNameMissing_ReportsError()
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var input =
@"enum UserType {
    User = 0,
    = 1
}";

            // Act
            var result = compiler.Compile(input.ToStream());

            // Assert
            var error = result.Messages.First();
            Assert.Equal(CompilerMessageId.EnumMemberMustHaveAName, error.MessageId);
            Assert.Equal(CompilerMessageType.Error, error.MessageType);
            Assert.Equal(3, error.LineNumber);
            Assert.Equal(5, error.StartPosition);
            Assert.Equal(7, error.EndPosition);
        }

        [Fact]
        public void Compile_EnumValueNegative_ReportsError()
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var input =
@"enum UserType {
    User = -1
}";

            // Act
            var result = compiler.Compile(input.ToStream());

            // Assert
            var error = result.Messages.First();
            Assert.Equal(CompilerMessageId.EnumValueMustNotBeNegative, error.MessageId);
            Assert.Equal(CompilerMessageType.Error, error.MessageType);
            Assert.Equal(2, error.LineNumber);
            Assert.Equal(12, error.StartPosition);
            Assert.Equal(13, error.EndPosition);
        }

        [Fact]
        public void Compile_EnumValueNotAnInteger_ReportsError()
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var input =
@"enum UserType {
    User = 'testing-123'
}";

            // Act
            var result = compiler.Compile(input.ToStream());

            // Assert
            var error = result.Messages.First();
            Assert.Equal(CompilerMessageId.EnumValueMustBeAnInteger, error.MessageId);
            Assert.Equal(CompilerMessageType.Error, error.MessageType);
            Assert.Equal(2, error.LineNumber);
            Assert.Equal(12, error.StartPosition);
            Assert.Equal(24, error.EndPosition);
        }

        [Fact]
        public void Compile_EnumValueMissing_ReportsError()
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var input =
@"enum UserType {
    User =
}";

            // Act
            var result = compiler.Compile(input.ToStream());

            // Assert
            var error = result.Messages.First();
            Assert.Equal(CompilerMessageId.EnumValueMustBeSpecified, error.MessageId);
            Assert.Equal(CompilerMessageType.Error, error.MessageType);
            Assert.Equal(2, error.LineNumber);
            Assert.Equal(5, error.StartPosition);
            Assert.Equal(10, error.EndPosition);
        }
    }
}