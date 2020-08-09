namespace Thrift.Net.Compilation
{
    /// <summary>
    /// The type (level) of message reported.
    /// </summary>
    public enum CompilerMessageType
    {
        /// <summary>
        /// A problem that should be addressed, but that doesn't prevent
        /// compilation from succeeding.
        /// </summary>
        Warning,

        /// <summary>
        /// A problem that must be addressed because it prevent compilation
        /// succeeding.
        /// </summary>
        Error,
    }
}