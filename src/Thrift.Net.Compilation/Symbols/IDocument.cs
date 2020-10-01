namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using Antlr4.Runtime.Tree;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Describes a Thrift IDL file.
    /// </summary>
    public interface IDocument : ISymbol<DocumentContext>
    {
        /// <summary>
        /// Gets the C# namespace of the document.
        /// </summary>
        IReadOnlyCollection<Namespace> Namespaces { get; }

        /// <summary>
        /// Gets any enums that have been defined.
        /// </summary>
        IReadOnlyCollection<Enum> Enums { get; }

        /// <summary>
        /// Gets any structs that have been defined.
        /// </summary>
        IReadOnlyCollection<Struct> Structs { get; }

        /// <summary>
        /// Gets all the types contained by this document, in the order they
        /// appeared in the source.
        /// </summary>
        IReadOnlyCollection<INamedSymbol> AllTypes { get; }

        /// <summary>
        /// Gets the C# namespace that should be used for generating this document.
        /// </summary>
        /// <remarks>
        /// If multiple valid namespace scopes are provided, the compiler will
        /// use the most specific namespace and fallback to the namespace with the
        /// `*` scope if no valid C# scopes are specified. If multiple C# scopes
        /// are specified (e.g. `csharp` and `netstd`), the last one defined in
        /// the document will be used.
        /// </remarks>
        string CSharpNamespace { get; }

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
