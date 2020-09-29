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
    }
}