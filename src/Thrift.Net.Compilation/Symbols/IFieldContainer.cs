namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a symbol that contains fields.
    /// </summary>
    public interface IFieldContainer : ISymbol
    {
        /// <summary>
        /// Gets the fields of the struct.
        /// </summary>
        IReadOnlyCollection<Field> Fields { get; }

        /// <summary>
        /// Gets the fields that are optional (either explicitly or implicitly).
        /// </summary>
        IReadOnlyCollection<Field> OptionalFields { get; }
    }
}