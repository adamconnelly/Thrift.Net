namespace Thrift.Net.Compilation
{
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
        public CompilationMessage(
            CompilerMessageId messageId,
            CompilerMessageType messageType,
            int lineNumber,
            int startPosition,
            int endPosition)
        {
            this.MessageId = messageId;
            this.MessageType = messageType;
            this.LineNumber = lineNumber;
            this.StartPosition = startPosition;
            this.EndPosition = endPosition;
        }

        /// <summary>
        /// Gets the type of message reported.
        /// </summary>
        public CompilerMessageId MessageId { get; }

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
    }
}