namespace Thrift.Net.Compilation
{
    using System.IO;
    using Antlr4.Runtime;
    using Thrift.Net.Antlr;

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
            var charStream = new AntlrInputStream(inputStream);
            var lexer = new ThriftLexer(charStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ThriftParser(tokenStream);

            var document = parser.document();

            var visitor = new CompilationVisitor();
            visitor.Visit(document);

            return new CompilationResult(
                new Model.ThriftDocument(visitor.Namespace, visitor.Enums),
                visitor.Messages);
        }
    }
}