namespace Thrift.Net.Compilation
{
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// Used to generate C# code from Thrift documents.
    /// </summary>
    public interface IThriftDocumentGenerator
    {
        /// <summary>
        /// Generates the C# required for the specified document.
        /// </summary>
        /// <param name="document">The document to generate code for.</param>
        /// <returns>The generated C#.</returns>
        string Generate(ThriftDocument document);
    }
}