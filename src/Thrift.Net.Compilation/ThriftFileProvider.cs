namespace Thrift.Net.Compilation
{
    using System.IO;

    /// <summary>
    /// Used to create instances of <see cref="ThriftFile" /> based on the compilation
    /// working directory and output directory.
    /// </summary>
    public class ThriftFileProvider : IThriftFileProvider
    {
        /// <summary>
        /// Creates a new thrift file taking into account the compilation working
        /// directory and output directory.
        /// </summary>
        /// <param name="workingDirectory">The compilation working directory.</param>
        /// <param name="inputFile">The file to be compiled.</param>
        /// <param name="outputDirectory">The file to output any generated code.</param>
        /// <returns>The thrift file.</returns>
        public ThriftFile Create(DirectoryInfo workingDirectory, FileInfo inputFile, DirectoryInfo outputDirectory)
        {
            var calculatedOutputDirectory = CalculateOutputDirectory(workingDirectory, inputFile, outputDirectory);

            return new ThriftFile(
                inputFile.Name,
                inputFile.FullName,
                Path.GetRelativePath(workingDirectory.FullName, inputFile.FullName),
                Path.Combine(calculatedOutputDirectory, inputFile.Name.Replace(inputFile.Extension, ".cs")));
        }

        private static string CalculateOutputDirectory(DirectoryInfo workingDirectory, FileInfo inputFile, DirectoryInfo outputDirectory)
        {
            var relativeOutputDirectory = Path.GetRelativePath(workingDirectory.FullName, inputFile.Directory.FullName);
            if (relativeOutputDirectory == ".")
            {
                return outputDirectory.FullName;
            }

            return Path.Combine(outputDirectory.FullName, relativeOutputDirectory);
        }
    }
}