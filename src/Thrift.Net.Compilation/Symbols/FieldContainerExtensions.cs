namespace Thrift.Net.Compilation.Symbols
{
    using System.Linq;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Contains extensions for the <see cref="IFieldContainer" /> type.
    /// </summary>
    public static class FieldContainerExtensions
    {
        /// <summary>
        /// Checks whether the field has already been defined.
        /// </summary>
        /// <param name="container">The field container to search.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="node">The field being defined.</param>
        /// <returns>
        /// true if the field name has already been defined, false otherwise.
        /// </returns>
        public static bool IsFieldNameAlreadyDefined(
            this IFieldContainer container,
            string name,
            FieldContext node)
        {
            return container.Fields
                .Where(item => item.Name == name)
                .FirstOrDefault().Node != node;
        }

        /// <summary>
        /// Checks whether a field with the specified Id has already been defined.
        /// </summary>
        /// <param name="container">The field container to search.</param>
        /// <param name="fieldId">The field Id to check for.</param>
        /// <param name="node">The field being defined.</param>
        /// <returns>
        /// true if the field Id has already been defined, false otherwise.
        /// </returns>
        public static bool IsFieldIdAlreadyDefined(
            this IFieldContainer container,
            int fieldId,
            FieldContext node)
        {
            return container.Fields
                .Where(item => item.FieldId == fieldId)
                .FirstOrDefault().Node != node;
        }
    }
}