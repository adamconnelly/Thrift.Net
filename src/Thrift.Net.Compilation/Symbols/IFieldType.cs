namespace Thrift.Net.Compilation.Symbols
{
    using Thrift.Net.Compilation.Types;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a field's type.
    /// </summary>
    public interface IFieldType : ISymbol<FieldTypeContext, IFieldContainer>
    {
        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the reference to the field's type.
        /// </summary>
        public ITypeReference Reference { get; }
    }
}