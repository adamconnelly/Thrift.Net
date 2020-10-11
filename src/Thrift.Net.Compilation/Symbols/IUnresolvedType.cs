namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Represents an unresolved type (i.e. where a type has been referenced, but
    /// no definition can be found).
    /// </summary>
    public interface IUnresolvedType : INamedTypeSymbol
    {
    }
}