namespace Thrift.Net.Compilation
{
    using System.Collections.Generic;
    using System.Linq;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// The result of compiling a Thrift document.
    /// </summary>
    public class CompilationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationResult" /> class.
        /// </summary>
        /// <param name="document">The thrift document.</param>
        /// <param name="messages">Any messages reported during compilation.</param>
        public CompilationResult(
            Document document,
            IReadOnlyCollection<CompilationMessage> messages)
        {
            this.Document = document;
            this.Messages = messages;
        }

        /// <summary>
        /// Gets a representation of the Thrift document.
        /// </summary>
        public Document Document { get; }

        /// <summary>
        /// Gets any messages reported during compilation.
        /// </summary>
        public IReadOnlyCollection<CompilationMessage> Messages { get; }

        /// <summary>
        /// Gets any errors.
        /// </summary>
        public IReadOnlyCollection<CompilationMessage> Errors =>
            this.Messages.Where(message => message.MessageType == CompilerMessageType.Error).ToList();

        /// <summary>
        /// Gets any warnings.
        /// </summary>
        public IReadOnlyCollection<CompilationMessage> Warnings =>
            this.Messages.Where(message => message.MessageType == CompilerMessageType.Warning).ToList();

        /// <summary>
        /// Gets a value indicating whether there were any errors compiling.
        /// </summary>
        public bool HasErrors => this.Errors.Any();

        /// <summary>
        /// Gets a value indicating whether there were any warnings identified
        /// while compiling.
        /// </summary>
        public bool HasWarnings => this.Warnings.Any();
    }
}