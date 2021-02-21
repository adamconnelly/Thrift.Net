namespace Thrift.Net.Compilation.Symbols
{
    using Thrift.Net.Compilation.Types;

    /// <summary>
    /// Represents the initialisation expression for a Thrift constant.
    /// </summary>
    public interface IConstantExpression : ISymbol
    {
        /// <summary>
        /// Gets the type of the constant's expression. For example, the constant
        /// declaration `const i32 MaxPageSize = 10` has a type of <see cref="BaseType.I8" />.
        /// </summary>
        IType Type { get; }

        /// <summary>
        /// Gets the raw text value of the constant definition as it was defined
        /// in the Thrift document.
        /// </summary>
        string RawValue { get; }
    }
}