namespace Thrift.Net.Compilation.Extensions
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Types;

    /// <summary>
    /// Extensions for <see cref="IFieldType" />.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks whether the specified type is an integer type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is an integer, false otherwise.</returns>
        public static bool IsIntegerType(this IType type)
        {
            return type.IsBaseType &&
                (type == BaseType.I8 ||
                type == BaseType.I16 ||
                type == BaseType.I32 ||
                type == BaseType.I64);
        }
    }
}