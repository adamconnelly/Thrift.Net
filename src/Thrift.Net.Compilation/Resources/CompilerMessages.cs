namespace Thrift.Net.Compilation.Resources
{
    using System.Resources;

    /// <summary>
    /// Provides access to compiler messages.
    /// </summary>
    public static class CompilerMessages
    {
        private static readonly ResourceManager Manager;

        static CompilerMessages()
        {
            Manager = new ResourceManager(
                "Thrift.Net.Compilation.Resources.CompilerMessages",
                typeof(CompilerMessages).Assembly);
        }

        /// <summary>
        /// Formats the specified compiler message Id as a string.
        /// </summary>
        /// <param name="messageId">The message Id.</param>
        /// <returns>The formatted message Id.</returns>
        public static string FormatMessageId(CompilerMessageId messageId)
        {
            return $"TC{(int)messageId:0000}";
        }

        /// <summary>
        /// Gets the content of the specified message Id.
        /// </summary>
        /// <param name="messageId">The Id of the message to get.</param>
        /// <returns>The content of the message.</returns>
        public static string Get(CompilerMessageId messageId)
        {
            return Manager.GetString(FormatMessageId(messageId));
        }
    }
}