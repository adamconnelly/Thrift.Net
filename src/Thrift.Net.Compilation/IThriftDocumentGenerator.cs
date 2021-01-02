namespace Thrift.Net.Compilation
{
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// Used to generate C# code from Thrift documents.
    /// </summary>
    public interface IThriftDocumentGenerator
    {
        /// <summary>
        /// Generates the C# representation of the specified Thrift document.
        /// </summary>
        /// <param name="file">Contains information about the thrift input file.</param>
        /// <param name="document">The document to generate code from.</param>
        /// <returns>The generated code.</returns>
        string Generate(ThriftFile file, IDocument document);
    }
}