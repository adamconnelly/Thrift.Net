namespace Thrift.Net.Compilation
{
    /// <summary>
    /// A list of messages output by the Thrift compiler.
    /// </summary>
    public enum CompilerMessageId
    {
        /// <summary>
        /// An enum has been defined without a name being specified.
        /// For example `enum {}`
        /// </summary>
        EnumMustHaveAName = 0,

        /// <summary>
        /// An enum member has been defined without a name being specified.
        /// For example `enum { = 1 }`.
        /// </summary>
        EnumMemberMustHaveAName = 1,

        /// <summary>
        /// An enum member has been defined with a negative value. For example
        /// `enum UserType { User = -1 }`.
        /// </summary>
        EnumValueMustNotBeNegative = 2,

        /// <summary>
        /// An enum member has been defined with a value that isn't an int. For
        /// example `enum UserType { User = 'hello' }`.
        /// </summary>
        EnumValueMustBeAnInteger = 3,

        /// <summary>
        /// An enum member has been defined but the value is missing from the
        /// assign expression. For example `enum UserType { User = }`.
        /// </summary>
        EnumValueMustBeSpecified = 4,

        /// <summary>
        /// The equals operator is missing between an enum member and its value.
        /// For example `enum UserType { User 1 }`.
        /// </summary>
        EnumMemberEqualsOperatorMissing = 5,
    }
}