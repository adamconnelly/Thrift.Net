namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="DocumentContext" /> nodes to <see cref="Document" />
    /// objects.
    /// </summary>
    public interface IDocumentBinder : IBinder
    {
        /// <summary>
        /// Checks whether an enum with the specified name has already been declared.
        /// </summary>
        /// <param name="enumName">The enum name.</param>
        /// <param name="enumNode">The node being declared.</param>
        /// <returns>
        /// true if an enum has with the same name has been declared before
        /// <paramref ref="enumNode" />. false otherwise.
        /// </returns>
        bool IsEnumAlreadyDeclared(string enumName, EnumDefinitionContext enumNode);
    }
}