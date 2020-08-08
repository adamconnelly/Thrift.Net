namespace Thrift.Net.Compilation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Antlr4.Runtime;
    using Thrift.Net.Antlr;
    using Thrift.Net.Compilation.Model;

    /// <summary>
    /// An object used to compile thrift IDL into a model that can be used for
    /// code generation.
    /// </summary>
    public class ThriftCompiler
    {
        /// <summary>
        /// Compiles the IDL contained in the input stream.
        /// </summary>
        /// <param name="inputStream">The stream containing the text to parse.</param>
        /// <returns>The result of the compile operation.</returns>
        public CompilationResult Compile(MemoryStream inputStream)
        {
            var charStream = new AntlrInputStream(inputStream);
            var lexer = new ThriftLexer(charStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ThriftParser(tokenStream);

            var document = parser.document();

            var visitor = new CompilationVisitor();
            visitor.Visit(document);

            return new CompilationResult(new Model.ThriftDocument(visitor.Enums));
        }
    }
}