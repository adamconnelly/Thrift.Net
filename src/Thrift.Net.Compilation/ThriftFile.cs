namespace Thrift.Net.Compilation
{
    /// <summary>
    /// Represents a `.thrift` file that can be compiled.
    /// </summary>
    public class ThriftFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThriftFile" /> class.
        /// </summary>
        /// <param name="fullName">The absolute path to the file.</param>
        /// <param name="relativePath">
        /// The path relative to the compilation working directory.
        /// </param>
        /// <param name="outputPath">
        /// The full path to where the output file should be generated.
        /// </param>
        public ThriftFile(string fullName, string relativePath, string outputPath)
        {
            this.FullName = fullName;
            this.RelativePath = relativePath;
            this.OutputPath = outputPath;
        }

        /// <summary>
        /// Gets the absolute path to the file.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets the path relative to the compilation working directory.
        /// </summary>
        public string RelativePath { get; }

        /// <summary>
        /// Gets the full path to where the output should be generated.
        /// </summary>
        public string OutputPath { get; }
    }
}