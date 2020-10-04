namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents an individual member of an enum.
    /// </summary>
    public interface IEnumMember : ISymbol<EnumMemberContext, IEnum>, INamedSymbol
    {
        /// <summary>
        /// Gets the enum value.
        /// </summary>
        int? Value { get; }

        /// <summary>
        /// Gets the raw text that represents the value of the enum.
        /// </summary>
        string RawValue { get; }

        /// <summary>
        /// Gets the reason (if any) that the enum value has failed to be parsed.
        /// </summary>
        InvalidEnumValueReason InvalidValueReason { get; }

        /// <summary>
        /// Gets a value indicating whether the member's value is implicit (i.e.
        /// generated automatically by the compiler instead of a value being provided
        /// for the member in the source).
        /// </summary>
        bool IsValueImplicit { get; }
    }
}