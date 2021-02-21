namespace Thrift.Net.Compilation.Types
{
    /// <summary>
    /// Represents a reference to a type. For example if a field is declared as
    /// `1: i32 Id` it has a reference to the `i32` base type.
    /// </summary>
    public interface ITypeReference
    {
        /// <summary>
        /// Gets a value indicating whether the type has successfully been
        /// resolved. If this is false it means the type could not be found.
        /// </summary>
        public bool IsResolved { get; }

        /// <summary>
        /// Gets the type being referenced.
        /// </summary>
        public IType Type { get; }
    }
}