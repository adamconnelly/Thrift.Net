namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using System.Linq;
    using Thrift.Net.Compilation;
    using Thrift.Net.Compilation.Resources;
    using Thrift.Net.Tests.Extensions;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public abstract class ThriftCompilerTests
    {
        protected void AssertCompilerReturnsError(
            string input, CompilerMessageId messageId)
        {
            this.AssertCompilerReturnsMessage(
                input, messageId, CompilerMessageType.Error);
        }

        protected void AssertCompilerReturnsWarning(
            string input, CompilerMessageId messageId)
        {
            this.AssertCompilerReturnsMessage(
                input, messageId, CompilerMessageType.Warning);
        }

        protected void AssertCompilerReturnsMessage(
            string input, CompilerMessageId messageId, CompilerMessageType messageType)
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var parserInput = ParserInput.FromString(input);

            // Act
            var result = compiler.Compile(parserInput.GetStream());

            // Assert
            var message = result.Messages.FirstOrDefault(m => m.MessageType == messageType);
            Assert.True(message != null, $"No {messageType.ToString().ToLower()} messages were returned from the compiler");
            Assert.Equal(messageId, message.MessageId);
            Assert.Equal(messageType, message.MessageType);
            Assert.Equal(parserInput.LineNumber, message.LineNumber);
            Assert.Equal(parserInput.StartPosition, message.StartPosition);
            Assert.Equal(parserInput.EndPosition, message.EndPosition);
        }

        protected void AssertCompilerReturnsErrorMessage(
            string input, CompilerMessageId messageId, params string[] messageParameters)
        {
            // Arrange
            var expectedMessage = string.Format(
                CompilerMessages.Get(messageId), messageParameters);
            var compiler = new ThriftCompiler();

            // Act
            var result = compiler.Compile(input.ToStream());

            // Assert
            Assert.Equal(expectedMessage, result.Errors.First().Message);
        }
    }
}