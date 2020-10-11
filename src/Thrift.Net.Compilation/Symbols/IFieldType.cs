namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Represents a field's type.
    /// </summary>
    public interface IFieldType : ISymbol
    {
        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the type has successfully been
        /// resolved. If this is false it means the type could not be found.
        /// </summary>
        public bool IsResolved { get; }

        /// <summary>
        /// Gets the name of the C# type to use when generating an optional field.
        /// </summary>
        public string CSharpOptionalTypeName { get; }

        /// <summary>
        /// Gets the name of the C# type to use when generating a required field.
        /// </summary>
        public string CSharpRequiredTypeName { get; }

        /// <summary>
        /// Gets a value indicating whether the type is a base (i.e. built-in) type.
        /// </summary>
        public bool IsBaseType { get; }

        /// <summary>
        /// Gets a value indicating whether the type is a struct.
        /// </summary>
        public bool IsStruct { get; }

        /// <summary>
        /// Gets a value indicating whether the type is an enum.
        /// </summary>
        public bool IsEnum { get; }
    }
}