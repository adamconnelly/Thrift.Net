namespace Thrift.Net.Compilation.Binding
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind enums.
    /// </summary>
    public interface IEnumBinder : IBinder
    {
        /// <summary>
        /// Gets the automatically generated value for an enum member. This is
        /// used where no value has been specified in the IDL.
        /// </summary>
        /// <param name="node">The member node to get the value for.</param>
        /// <returns>The enum value.</returns>
        /// <remarks>
        /// According to the Thrift IDL specification, if an enum value is
        /// not supplied, it should either be:
        /// - 0 for the first element in an enum.
        /// - P+1 - where `P` is the value of the previous element.
        /// </remarks>
        int GetEnumValue(EnumMemberContext node);
    }
}