namespace Thrift.Net.Compilation.Binding
{
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="DocumentContext" /> nodes to <see cref="Document" />
    /// objects.
    /// </summary>
    public interface IDocumentBinder : IBinder
    {
        /// <summary>
        /// Checks whether another member with the specified name has already
        /// been declared in the document.
        /// </summary>
        /// <param name="memberName">The member's name.</param>
        /// <param name="memberNode">The node being declared.</param>
        /// <returns>
        /// true if another member with the same name has been declared before
        /// <paramref name="memberNode" />. false otherwise.
        /// </returns>
        bool IsMemberNameAlreadyDeclared(string memberName, IParseTree memberNode);

        /// <summary>
        /// Checks whether another namespace statement has already been declared
        /// for the same scope.
        /// </summary>
        /// <param name="namespace">The namespace being declared.</param>
        /// <returns>
        /// true if another namespace statement has already been declared with
        /// the same scope. false otherwise.
        /// </returns>
        bool IsNamespaceForScopeAlreadyDeclared(Namespace @namespace);
    }
}