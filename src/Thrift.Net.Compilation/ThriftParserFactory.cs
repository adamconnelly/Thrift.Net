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
        /// <param name="errorListener">
        /// Used to collect any errors encountered during lexing or parsing. If
        /// no listener is provided, the default Antlr listeners that output
        /// messages to the console will be used.
        /// </param>
        /// <returns>The parser.</returns>
        public static ThriftParser Create(
            Stream inputStream,
            CollectingErrorListener errorListener = null)
        {
            var charStream = new AntlrInputStream(inputStream);
            var lexer = new ThriftLexer(charStream);
            var tokenStream = new CommonTokenStream(lexer);

            ThriftParser parser = new ThriftParser(tokenStream);

            if (errorListener != null)
            {
                lexer.RemoveErrorListeners();
                lexer.AddErrorListener(errorListener);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);
            }

            return parser;
        }
    }
}