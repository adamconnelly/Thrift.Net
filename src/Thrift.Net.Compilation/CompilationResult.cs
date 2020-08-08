namespace Thrift.Net.Compilation
{
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
        public CompilationResult(ThriftDocument document)
        {
            this.Document = document;
        }

        /// <summary>
        /// Gets a representation of the Thrift document.
        /// </summary>
        public ThriftDocument Document { get; }
    }
}