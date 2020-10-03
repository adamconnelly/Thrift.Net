namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a namespace statement.
    /// </summary>
    public interface INamespace : ISymbol<NamespaceStatementContext>
    {
        /// <summary>
        /// Gets the scope of the namespace.
        /// </summary>
        string Scope { get; }

        /// <summary>
        /// Gets a value indicating whether the namespace declaration's scope is
        /// one that can be used for C# code generation.
        /// </summary>
        bool HasCSharpScope { get; }

        /// <summary>
        /// Gets a value indicating whether the scope is known by the compiler.
        /// </summary>
        bool HasKnownScope { get; }

        /// <summary>
        /// Gets a value indicating whether this namespace applies to all code
        /// generation targets (i.e. has the `*` scope).
        /// </summary>
        bool AppliesToAllTargets { get; }

        /// <summary>
        /// Gets the namespace name.
        /// </summary>
        string NamespaceName { get; }
    }
}