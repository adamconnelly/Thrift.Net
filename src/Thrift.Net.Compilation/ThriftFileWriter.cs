namespace Thrift.Net.Compilation
{
    using System.IO;

    /// <inheritdoc />.
    public class ThriftFileWriter : IThriftFileWriter
    {
        /// <inheritdoc />.
        public void Write(ThriftFile thriftFile, string generatedCode)
        {
            var outputFile = new FileInfo(thriftFile.OutputPath);
            if (!outputFile.Directory.Exists)
            {
                outputFile.Directory.Create();
            }

            using (var writer = new StreamWriter(outputFile.OpenWrite()))
            {
                writer.WriteLine(generatedCode);
            }
        }
    }
}