namespace Thrift.Net.Compilation
{
    using System.Collections.Generic;
    using Thrift.Net.Compilation.Model;

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
            ThriftDocument document,
            IReadOnlyCollection<CompilationMessage> messages)
        {
            this.Document = document;
            this.Messages = messages;
        }

        /// <summary>
        /// Gets a representation of the Thrift document.
        /// </summary>
        public ThriftDocument Document { get; }

        /// <summary>
        /// Gets any messages reported during compilation.
        /// </summary>
        public IReadOnlyCollection<CompilationMessage> Messages { get; }
    }
}