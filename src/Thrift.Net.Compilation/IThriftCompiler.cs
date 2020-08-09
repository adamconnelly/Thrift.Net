namespace Thrift.Net.Compilation
{
    using System.IO;

    /// <summary>
    /// Used to compile Thrift documents.
    /// </summary>
    public interface IThriftCompiler
    {
        /// <summary>
        /// Compiles a Thrift document.
        /// </summary>
        /// <param name="inputStream">The text to compile.</param>
        /// <returns>The result of the compilation.</returns>
        CompilationResult Compile(Stream inputStream);
    }
}