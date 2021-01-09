namespace Thrift.Net.Compilation.Extensions
{
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// Extensions for <see cref="IFieldType" />.
    /// </summary>
    public static class FieldTypeExtensions
    {
        /// <summary>
        /// Checks whether the specified type is an integer type.
        /// </summary>
        /// <param name="fieldType">The type to check.</param>
        /// <returns>true if the type is an integer, false otherwise.</returns>
        public static bool IsIntegerType(this IFieldType fieldType)
        {
            return fieldType.IsBaseType &&
                (fieldType.Name == BaseType.I8Name ||
                fieldType.Name == BaseType.I16Name ||
                fieldType.Name == BaseType.I32Name ||
                fieldType.Name == BaseType.I64Name);
        }
    }
}