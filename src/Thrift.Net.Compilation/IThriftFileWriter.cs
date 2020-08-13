namespace Thrift.Net.Compilation
{
    /// <summary>
    /// Writes generated code for the specified Thrift file. Used to allow us
    /// to write tests without worrying about them writing out files.
    /// </summary>
    public interface IThriftFileWriter
    {
        /// <summary>
        /// Writes the generated code to the output of the thrift file.
        /// </summary>
        /// <param name="thriftFile">
        /// Contains information about where to write the file.
        /// </param>
        /// <param name="generatedCode">The code to write to the file.</param>
        void Write(ThriftFile thriftFile, string generatedCode);
    }
}