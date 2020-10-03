namespace Thrift.Net.Compilation
{
    using System.IO;
    using System.Linq;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;

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

            var documentNode = parser.document();
            var document = BinderProvider.Instance
                .GetBinder(documentNode)
                .Bind<IDocument>(documentNode, null);

            var visitor = new CompilationVisitor();
            document.Accept(visitor);

            var combinedMessages = visitor.Messages
                .Union(parserErrorListener.Messages)
                .ToList();

            return new CompilationResult(document, combinedMessages);
        }
    }
}
