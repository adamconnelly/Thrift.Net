namespace Thrift.Net.Compilation
{
    using System.Collections.Generic;
    using System.IO;
    using Antlr4.Runtime;

    /// <summary>
    /// Collects any errors reported by Antlr during parsing and converts them
    /// to <see cref="CompilationMessage" /> objects.
    /// </summary>
    public class CollectingErrorListener : IAntlrErrorListener<IToken>, IAntlrErrorListener<int>
    {
        private readonly List<CompilationMessage> messages = new List<CompilationMessage>();

        /// <summary>
        /// Gets any errors reported during parsing.
        /// </summary>
        public IReadOnlyCollection<CompilationMessage> Messages => this.messages;

        /// <inheritdoc />
        public void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            IToken offendingSymbol,
            int line,
            int charPositionInLine,
            string message,
            RecognitionException exception)
        {
            this.messages.Add(
                CompilationMessage.CreateError(
                    CompilerMessageId.GenericParseError,
                    offendingSymbol,
                    offendingSymbol,
                    new[] { message }));
        }

        /// <inheritdoc />
        public void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            int offendingSymbol,
            int line,
            int charPositionInLine,
            string message,
            RecognitionException exception)
        {
            this.messages.Add(
                new CompilationMessage(
                    CompilerMessageId.GenericParseError,
                    CompilerMessageType.Error,
                    line,
                    charPositionInLine,
                    charPositionInLine,
                    message));
        }
    }
}