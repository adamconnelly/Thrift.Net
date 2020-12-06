namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Describes a Thrift IDL file.
    /// </summary>
    public interface IDocument : ISymbol<DocumentContext, ISymbol>
    {
        /// <summary>
        /// Gets the C# namespace of the document.
        /// </summary>
        IReadOnlyCollection<INamespace> Namespaces { get; }

        /// <summary>
        /// Gets any enums that have been defined.
        /// </summary>
        IReadOnlyCollection<IEnum> Enums { get; }

        /// <summary>
        /// Gets any structs that have been defined.
        /// </summary>
        IReadOnlyCollection<IStruct> Structs { get; }

        /// <summary>
        /// Gets any unions that have been defined.
        /// </summary>
        IReadOnlyCollection<IUnion> Unions { get; }

        /// <summary>
        /// Gets any exceptions that have been defined.
        /// </summary>
        IReadOnlyCollection<IException> Exceptions { get; }

        /// <summary>
        /// Gets all the types contained by this document, in the order they
        /// appeared in the source.
        /// </summary>
        IReadOnlyCollection<INamedTypeSymbol> AllTypes { get; }

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
        /// Gets a value indicating whether the Document contains any definitions
        /// that can be generated.
        /// </summary>
        bool ContainsDefinitions { get; }

        /// <summary>
        /// Checks whether another member with the specified name has already
        /// been declared in the document.
        /// </summary>
        /// <param name="member">The member that is potentially a duplicate.</param>
        /// <returns>
        /// true if another member with the same name has been declared before
        /// <paramref name="member" />. false otherwise.
        /// </returns>
        bool IsMemberNameAlreadyDeclared(INamedSymbol member);

        /// <summary>
        /// Checks whether another namespace statement has already been declared
        /// for the same scope.
        /// </summary>
        /// <param name="namespace">The namespace being declared.</param>
        /// <returns>
        /// true if another namespace statement has already been declared with
        /// the same scope. false otherwise.
        /// </returns>
        bool IsNamespaceForScopeAlreadyDeclared(INamespace @namespace);
    }
}
