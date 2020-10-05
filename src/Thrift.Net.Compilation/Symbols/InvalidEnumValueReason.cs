namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Describes the reason (if any) that an enum value has failed to be parsed.
    /// </summary>
    public enum InvalidEnumValueReason
    {
        /// <summary>
        /// The enum value is not invalid.
        /// </summary>
        None,

        /// <summary>
        /// The enum value is missing (but the `=` operator has been specified).
        /// </summary>
        Missing,

        /// <summary>
        /// The enum value is not an integer.
        /// </summary>
        NotAnInteger,

        /// <summary>
        /// The enum value is negative.
        /// </summary>
        Negative,

        /// <summary>
        /// The enum value starts with the hex prefix (`0x`), but is not a valid
        /// hex value.
        /// </summary>
        InvalidHexValue,
    }
}