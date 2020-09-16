namespace Thrift.Net.Compilation
{
    using System.IO;
    using System.Linq;
    using Thrift.Net.Compilation.Binding;

    /// <summary>
    /// An object used to compile thrift IDL into a model that can be used for
    /// code generation.
    /// </summary>
    public class ThriftCompiler : IThriftCompiler
    {
        /// <summary>
        /// Compiles the IDL contained in the input stream.
        /// </summary>
        /// <param name="inputStream">The stream containing the text to parse.</param>
        /// <returns>The result of the compile operation.</returns>
        public CompilationResult Compile(Stream inputStream)
        {
            var parserErrorListener = new CollectingErrorListener();
            var parser = ThriftParserFactory.Create(inputStream, parserErrorListener);

            var document = parser.document();

            var binderProvider = new BinderProvider();
            binderProvider.AddDocument(document);

            var visitor = new CompilationVisitor(binderProvider);
            visitor.Visit(document);

            var combinedMessages = visitor.Messages
                .Union(parserErrorListener.Messages)
                .ToList();

            return new CompilationResult(visitor.Document, combinedMessages);
        }
    }
}
