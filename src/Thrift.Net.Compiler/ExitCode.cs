namespace Thrift.Net.Compiler
{
    /// <summary>
    /// Defines the exit codes returned by the Thrift compiler.
    /// </summary>
    public enum ExitCode
    {
        /// <summary>
        /// Compilation completed successfully.
        /// </summary>
        Success = 0,

        /// <summary>
        /// The specified input file was not found.
        /// </summary>
        InputFileNotFound = 1,

        /// <summary>
        /// Compilation has failed because of one or more errors.
        /// </summary>
        CompilationFailed = 2,

        /// <summary>
        /// Compilation has failed because of an unhandled exception.
        /// </summary>
        UnhandledException = 3,
    }
}