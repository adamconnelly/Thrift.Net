namespace Thrift.Net.Compilation.Symbols
{
    /// <summary>
    /// Defines whether or not a field is required.
    /// </summary>
    public enum FieldRequiredness
    {
        /// <summary>
        /// The default (implicit) level of requiredness if none is explicitly
        /// specified.
        /// </summary>
        /// <remarks>
        /// - Write: In theory, the fields are always written, however there are
        ///   some exceptions to this rule, including when the default value of
        ///   the field cannot be serialized by Thrift.
        /// - Read: Like optional, the field may, or may not be part of the input stream.
        /// - Default values: may or may not be written.
        /// </remarks>
        Default,

        /// <summary>
        /// The field is required.
        /// </summary>
        /// <remarks>
        /// - Write: Required fields are always written and are expected to be set.
        /// - Read: Required fields are always read and are expected to be contained
        ///   in the input stream.
        /// - Default values: are always written.
        /// </remarks>
        Required,

        /// <summary>
        /// The field is optional.
        /// </summary>
        /// <remarks>
        /// - Write: Optional fields are only written when they are set.
        /// - Read: Optional fields may, or may not be part of the input stream.
        /// - Default values: are written when the isset flag is set.
        /// </remarks>
        Optional,
    }
}