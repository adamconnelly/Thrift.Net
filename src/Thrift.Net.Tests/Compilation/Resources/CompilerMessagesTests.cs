namespace Thrift.Net.Tests.Compilation.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Resources;
    using Thrift.Net.Compilation;
    using Thrift.Net.Compilation.Resources;
    using Xunit;

    public class CompilerMessagesTests
    {
        private readonly ResourceManager resourceManager = new ResourceManager(
            "Thrift.Net.Compilation.Resources.CompilerMessages",
            typeof(CompilerMessages).Assembly);

        public static IEnumerable<object[]> GetCompilerMessageIds()
        {
            foreach (var messageId in Enum.GetValues(typeof(CompilerMessageId)))
            {
                yield return new object[] { messageId };
            }
        }

        [Theory]
        [MemberData(nameof(GetCompilerMessageIds))]
        public void GetMessage_ForAllMessageIds_ReturnsMessage(CompilerMessageId messageId)
        {
            // Arrange
            var messageKey = CompilerMessages.FormatMessageId(messageId);
            var expectedMessage = this.resourceManager.GetString(messageKey);

            // Act
            var actualMessage = CompilerMessages.Get(messageId);

            // Assert
            Assert.True(!string.IsNullOrEmpty(expectedMessage), $"Compiler message missing for {messageKey}");
            Assert.Equal(expectedMessage, actualMessage);
        }
    }
}