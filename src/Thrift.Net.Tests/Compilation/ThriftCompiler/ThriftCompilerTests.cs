namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using System.Linq;
    using Thrift.Net.Compilation;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public abstract class ThriftCompilerTests
    {
        protected void AssertCompilerReturnsError(
            string input, CompilerMessageId messageId)
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var parserInput = ParserInput.FromString(input);

            // Act
            var result = compiler.Compile(parserInput.GetStream());

            // Assert
            var error = result.Errors.FirstOrDefault();
            Assert.True(error != null, "No error messages were returned from the compiler");
            Assert.Equal(messageId, error.MessageId);
            Assert.Equal(CompilerMessageType.Error, error.MessageType);
            Assert.Equal(parserInput.LineNumber, error.LineNumber);
            Assert.Equal(parserInput.StartPosition, error.StartPosition);
            Assert.Equal(parserInput.EndPosition, error.EndPosition);
        }
    }
}