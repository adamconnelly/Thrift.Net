namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using System.Linq;
    using Thrift.Net.Compilation;
    using Thrift.Net.Compilation.Resources;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public abstract class ThriftCompilerTests
    {
        protected void AssertCompilerReturnsMessage(
            string input,
            CompilerMessageId messageId,
            CompilerMessageType messageType,
            string[] messageParameters)
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var parserInput = ParserInput.FromString(input);

            // Act
            var result = compiler.Compile(parserInput.GetStream());

            // Assert
            var message = result.Messages.FirstOrDefault(
                m => m.MessageType == messageType && m.MessageId == messageId);
            Assert.True(message != null, $"No {messageType.ToString().ToLower()} messages were returned from the compiler");

            if (parserInput.LineNumber != null)
            {
                Assert.Equal(parserInput.LineNumber, message.LineNumber);
                Assert.Equal(parserInput.StartPosition, message.StartPosition);
                Assert.Equal(parserInput.EndPosition, message.EndPosition);
            }

            if (messageParameters?.Length > 0)
            {
                var expectedMessage = string.Format(
                    CompilerMessages.Get(messageId), messageParameters);
                Assert.Equal(expectedMessage, message.Message);
            }
        }

        protected void AssertCompilerReturnsErrorMessage(
            string input, CompilerMessageId messageId, params string[] messageParameters)
        {
            this.AssertCompilerReturnsMessage(
                input,
                messageId,
                CompilerMessageType.Error,
                messageParameters);
        }

        protected void AssertCompilerReturnsWarningMessage(
            string input, CompilerMessageId messageId, params string[] messageParameters)
        {
            this.AssertCompilerReturnsMessage(
                input,
                messageId,
                CompilerMessageType.Warning,
                messageParameters);
        }
    }
}