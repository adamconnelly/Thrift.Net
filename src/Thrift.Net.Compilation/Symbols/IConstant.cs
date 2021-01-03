namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Represents a Thrift constant.
    /// </summary>
    public interface IConstant : INamedTypeSymbol
    {
        /// <summary>
        /// Gets the type the constant has been declared as. For example, the constant
        /// declaration `const i32 MaxPageSize = 100` has a type of <see cref="BaseType.I32" />.
        /// </summary>
        IFieldType Type { get; }

        /// <summary>
        /// Gets the value of the constant definition.
        /// </summary>
        IConstantExpression Value { get; }
    }
}