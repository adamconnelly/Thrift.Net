namespace Thrift.Net.Compilation
{
    using System.IO;

    /// <summary>
    /// Creates <see cref="ThriftFile" /> objects based on a working directory,
    /// input file and output directory. The <see cref="ThriftFile" /> object
    /// provides information about where the file is relative to the working
    /// directory, along with where its compilation output should go.
    /// </summary>
    public interface IThriftFileProvider
    {
        /// <summary>
        /// Creates a new <see cref="ThriftFile" />.
        /// </summary>
        /// <param name="workingDirectory">The working directory of the compiler.</param>
        /// <param name="inputFile">The file that is being compiled.</param>
        /// <param name="outputDirectory">The output directory for the generated code.</param>
        /// <returns>The file information.</returns>
        ThriftFile Create(DirectoryInfo workingDirectory, FileInfo inputFile, DirectoryInfo outputDirectory);
    }
}