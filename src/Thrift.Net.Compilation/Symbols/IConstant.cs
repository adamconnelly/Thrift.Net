namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift constant.
    /// </summary>
    public interface IConstant : ISymbol<ConstDefinitionContext, IDocument>, INamedTypeSymbol
    {
        /// <summary>
        /// Gets the type the constant has been declared as. For example, the constant
        /// declaration `const i32 MaxPageSize = 100` has a type of <see cref="BaseType.I32" />.
        /// </summary>
        IFieldType Type { get; }

        /// <summary>
        /// Gets the expression used to initialize the constant.
        /// </summary>
        IConstantExpression Expression { get; }

        /// <summary>
        /// Gets the CSharp value of the constant expression that should be used for
        /// code generation.
        /// </summary>
        string CSharpValue { get; }
    }
}