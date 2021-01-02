namespace Thrift.Net.Compilation
{
    using System;

    /// <summary>
    /// Represents a `.thrift` file that can be compiled.
    /// </summary>
    public class ThriftFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThriftFile" /> class.
        /// </summary>
        /// <param name="fileName">The filename without the path.</param>
        /// <param name="fullName">The absolute path to the file.</param>
        /// <param name="relativePath">
        /// The path relative to the compilation working directory.
        /// </param>
        /// <param name="outputPath">
        /// The full path to where the output file should be generated.
        /// </param>
        public ThriftFile(string fileName, string fullName, string relativePath, string outputPath)
        {
            this.FileName = fileName;
            this.FullName = fullName;
            this.RelativePath = relativePath;
            this.OutputPath = outputPath;
        }

        /// <summary>
        /// Gets the filename (without the path).
        /// </summary>
        public string FileName { get; }

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

        /// <summary>
        /// Gets the name to use when generating a constants class for this file.
        /// </summary>
        public string ConstantsClassName
        {
            get
            {
                var name = this.FileName
                    .Replace(".thrift", string.Empty)
                    .Replace(".", "_")
                    .Replace("-", "_")
                    .Replace("+", "_");

                if (name.EndsWith("constants", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Make sure we uppercase "constants" and replace any invalid characters
                    return name.Replace("constants", "Constants");
                }

                return name + "Constants";
            }
        }
    }
}