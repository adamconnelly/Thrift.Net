namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// A Binder that binds elements containing fields.
    /// </summary>
    public interface IFieldContainerBinder : IBinder
    {
        /// <summary>
        /// Gets the default requiredness for this type of container.
        /// </summary>
        /// <remarks>
        /// This is required because unions have a different default requireness
        /// than structs or exceptions.
        /// </remarks>
        FieldRequiredness DefaultFieldRequiredness { get; }

        /// <summary>
        /// Checks whether a field with the specified Id has already been defined.
        /// </summary>
        /// <param name="fieldId">The field Id to check for.</param>
        /// <param name="node">The field being defined.</param>
        /// <returns>
        /// true if the field Id has already been defined, false otherwise.
        /// </returns>
        bool IsFieldIdAlreadyDefined(int fieldId, FieldContext node);

        /// <summary>
        /// Checks whether the field has already been defined.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="node">The field being defined.</param>
        /// <returns>
        /// true if the field name has already been defined, false otherwise.
        /// </returns>
        bool IsFieldNameAlreadyDefined(string name, FieldContext node);

        /// <summary>
        /// Gets the automatic field Id to use for the node in the situation
        /// where an explicit field Id has not been provided.
        /// </summary>
        /// <param name="node">The field to get an automatic Id for.</param>
        /// <returns>The next available field Id.</returns>
        int GetAutomaticFieldId(FieldContext node);
    }
}