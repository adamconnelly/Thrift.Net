namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Model;
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
        /// Gets the field defined immediately before the specified field.
        /// </summary>
        /// <param name="context">The field to look before.</param>
        /// <returns>The previous field, or null if there is no previous field.</returns>
        FieldDefinition GetPreviousSibling(FieldContext context);
    }
}