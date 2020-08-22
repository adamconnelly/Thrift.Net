namespace Thrift.Net.Compilation
{
    using Thrift.Net.Compilation.Resources;

    /// <summary>
    /// Represents a message output by the Thrift compiler.
    /// </summary>
    public class CompilationMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationMessage" /> class.
        /// </summary>
        /// <param name="messageId">The compiler message Id.</param>
        /// <param name="messageType">The type of message being reported.</param>
        /// <param name="lineNumber">The 1-based line number in the document.</param>
        /// <param name="startPosition">
        /// The 1-based starting position of the message on the line.
        /// </param>
        /// <param name="endPosition">
        /// The 1-based ending position of the message on the line.
        /// </param>
        /// <param name="message">
        /// The user-friendly compiler message.
        /// </param>
        public CompilationMessage(
            CompilerMessageId messageId,
            CompilerMessageType messageType,
            int lineNumber,
            int startPosition,
            int endPosition,
            string message)
        {
            this.MessageId = messageId;
            this.MessageType = messageType;
            this.LineNumber = lineNumber;
            this.StartPosition = startPosition;
            this.EndPosition = endPosition;
            this.Message = message;
        }

        /// <summary>
        /// Gets the type of message reported.
        /// </summary>
        public CompilerMessageId MessageId { get; }

        /// <summary>
        /// Gets the message Id formatted for output.
        /// </summary>
        public string FormattedMessageId => CompilerMessages.FormatMessageId(this.MessageId);

        /// <summary>
        /// Gets the type (level) of message reported.
        /// </summary>
        public CompilerMessageType MessageType { get; }

        /// <summary>
        /// Gets the line number in the document the error is being reported.
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// Gets the 1-based starting position of the message in the line.
        /// </summary>
        public int StartPosition { get; }

        /// <summary>
        /// Gets the 1-based ending position of the message in the line.
        /// </summary>
        public int EndPosition { get; }

        /// <summary>
        /// Gets the user-friendly message.
        /// </summary>
        public string Message { get; }
    }
}