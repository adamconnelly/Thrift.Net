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

            // Although we know that message cannot be null because of the assert on
            // the previous line, check for null here because the lgtm check isn't
            // smart enough to realise that
            if (message != null)
            {
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

        protected void AssertCompilerDoesNotReturnMessage(
            string input,
            CompilerMessageId messageId)
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var parserInput = ParserInput.FromString(input);

            // Act
            var result = compiler.Compile(parserInput.GetStream());

            // Assert
            var message = result.Messages.FirstOrDefault(m => m.MessageId == messageId);
            Assert.True(
                message == null,
                $"Message Id '{messageId}' should not have been reported");
        }

        protected void AssertCompilerDoesNotReturnAnyMessages(string input)
        {
            // Arrange
            var compiler = new ThriftCompiler();
            var parserInput = ParserInput.FromString(input);

            // Act
            var result = compiler.Compile(parserInput.GetStream());

            // Assert
            Assert.Empty(result.Messages);
        }
    }
}