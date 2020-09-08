namespace Thrift.Net.Compilation
{
    using System.IO;
    using Antlr4.Runtime;
    using Thrift.Net.Antlr;

    /// <summary>
    /// Used to create <see cref="ThriftParser" /> instances.
    /// </summary>
    public static class ThriftParserFactory
    {
        /// <summary>
        /// Creates a new parser from the specified input stream.
        /// </summary>
        /// <param name="inputStream">The stream to parse.</param>
        /// <returns>The parser.</returns>
        public static ThriftParser Create(Stream inputStream)
        {
            var charStream = new AntlrInputStream(inputStream);
            var lexer = new ThriftLexer(charStream);
            var tokenStream = new CommonTokenStream(lexer);

            return new ThriftParser(tokenStream);
        }
    }
}