namespace Thrift.Net.Tests.Utility
{
    using System;
    using System.IO;
    using Thrift.Net.Antlr;
    using Thrift.Net.Compilation;
    using Thrift.Net.Tests.Extensions;

    /// <summary>
    /// Provides a way of intepreting string examples in our tests so that we
    /// can easily identify the place in the text that errors should appear.
    /// </summary>
    public class ParserInput
    {
        public ParserInput(
            string input,
            int lineNumber,
            int startPosition,
            int endPosition)
        {
            this.Input = input;
            this.LineNumber = lineNumber;
            this.StartPosition = startPosition;
            this.EndPosition = endPosition;
        }

        /// <summary>
        /// Gets the input string with any `$` symbols removed.
        /// </summary>
        public string Input { get; }

        /// <summary>
        /// Gets the line number in the input we expect a message to appear.
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// Gets the start position in the line where we expect a message to
        /// appear.
        /// </summary>
        public int StartPosition { get; }

        /// <summary>
        /// Gets the end position in the line where we expect the message to
        /// appear. NOTE: this is the position of the last character, rather
        /// than the character after it.
        /// </summary>
        public int EndPosition { get; }

        /// <summary>
        /// Creates a new <see cref="ParserInput" /> object from the specified
        /// input. The input can contain `$` delimeters to identify the part
        /// of the string we expect to have a message added to, for example
        /// `enum $MyEnum$ {}`.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// Contains a representation of the input that can be used for testing.
        /// </returns>
        public static ParserInput FromString(string input)
        {
            var lineNumber = 0;
            var startPosition = 0;
            var endPosition = 0;
            var reader = new StringReader(input);
            var currentLine = 1;

            string line;
            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    if (line.Contains("$"))
                    {
                        lineNumber = currentLine;
                        startPosition = line.IndexOf("$") + 1;
                        endPosition = line.LastIndexOf("$") - 1;
                        break;
                    }

                    currentLine++;
                }
            }
            while (line != null);

            return new ParserInput(
                input.Replace("$", string.Empty),
                lineNumber,
                startPosition,
                endPosition);
        }

        /// <summary>
        /// Gets a <see cref="Stream" /> from the input.
        /// </summary>
        /// <returns>A stream created from the input string.</returns>
        public Stream GetStream() => this.Input.ToStream();

        public TNode ParseInput<TNode>(Func<ThriftParser, TNode> parseMethod)
        {
            var parser = ThriftParserFactory.Create(this.GetStream());

            return parseMethod(parser);
        }
    }
}
