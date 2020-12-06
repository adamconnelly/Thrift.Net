namespace Thrift.Net.Compilation
{
    /// <summary>
    /// A list of messages output by the Thrift compiler.
    /// </summary>
    public enum CompilerMessageId
    {
        /// <summary>
        /// An enum has been defined without a name being specified.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum {
        /// ^^^^
        ///   User,
        ///   Administrator
        /// }
        /// </code>
        ///
        /// To fix this add a name to the enum:
        /// <code>
        /// enum UserType {
        ///   User,
        ///   Administrator
        /// }
        /// </code>
        /// </example>
        EnumMustHaveAName = 0,

        /// <summary>
        /// An enum member has been defined without a name being specified.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///  User = 0,
        ///   = 1
        ///   ^^^
        /// }
        /// </code>
        ///
        /// To fix this add a name to the member:
        /// <code>
        /// enum UserType {
        ///  User = 0,
        ///  Administrator = 1
        /// }
        /// </code>
        /// </example>
        EnumMemberMustHaveAName = 1,

        /// <summary>
        /// An enum member has been defined with a negative value.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///  User = 0,
        ///  Administrator = -1
        ///                  ^^
        /// }
        /// </code>
        ///
        /// To fix this use a positive value:
        /// <code>
        /// enum UserType {
        ///  User = 0,
        ///  Administrator = 1
        /// }
        /// </code>
        /// </example>
        EnumValueMustNotBeNegative = 2,

        /// <summary>
        /// An enum member has been defined with a value that isn't an int.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///  User = 0,
        ///  Administrator = "hello"
        ///                  ^^^^^^^
        /// }
        /// </code>
        ///
        /// To fix this change the value to an integer:
        /// <code>
        /// enum UserType {
        ///  User = 0,
        ///  Administrator = 1
        /// }
        /// </code>
        /// </example>
        EnumValueMustBeAnInteger = 3,

        /// <summary>
        /// An enum member has been defined but the value is missing from the
        /// assign expression.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///   User =
        ///   ^^^^^^
        /// }
        /// </code>
        ///
        /// To fix this assign a value:
        /// <code>
        /// enum UserType {
        ///   User = 0
        /// }
        /// </code>
        /// </example>
        EnumValueMustBeSpecified = 4,

        /// <summary>
        /// The equals operator is missing between an enum member and its value.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///   User 1
        ///   ^^^^^^
        /// }
        /// </code>
        ///
        /// To fix this issue, add the equals operator:
        /// <code>
        /// enum UserType {
        ///   User = 1
        /// }
        /// </code>
        /// </example>
        EnumMemberEqualsOperatorMissing = 5,

        /// <summary>
        /// An enum has been defined with no members.
        /// </summary>
        /// <example>
        /// The following example produces this warning:
        /// <code>
        /// enum UserType {
        /// }
        /// </code>
        ///
        /// To fix this issue, add at least one enum member:
        /// <code>
        /// enum UserType {
        ///   User = 0
        /// }
        /// </code>
        /// </example>
        EnumEmpty = 6,

        /// <summary>
        /// The same enum member has been declared multiple times.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///  User = 0,
        ///  User = 1
        ///  ^^^^
        /// }
        /// </code>
        ///
        /// To fix this issue rename or remove the duplicate:
        /// <code>
        /// enum UserType {
        ///  User = 0,
        ///  Administrator = 1
        /// }
        /// </code>
        /// </example>
        EnumMemberDuplicated = 7,

        /// <summary>
        /// Another item has already been declared with the same name.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {}
        /// struct UserType {}
        /// </code>
        ///
        /// To fix this issue rename or remove the duplicate:
        /// <code>
        /// enum UserType {}
        /// </code>
        /// </example>
        NameAlreadyDeclared = 8,

        /// <summary>
        /// An enum member has been specified, but does not have an explicit value
        /// defined. This means that a value will be generated implicitly, which
        /// can lead to backwards-incompatible changes if the enum members are reordered.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///   Administrator = 0
        ///   User
        ///   ^^^^
        /// }
        /// </code>
        ///
        /// To fix this issue, specify a value for the enum member:
        /// <code>
        /// enum UserType {
        ///   Administrator = 0
        ///   User = 1
        /// }
        /// </code>
        /// </example>
        EnumMemberHasImplicitValue = 9,

        /// <summary>
        /// An enum value has been specified with the hex specifier (`0x`), but
        /// it is not a valid hex value.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///   Administrator = 0xZZZ
        ///                   ^^^^^
        /// }
        /// </code>
        ///
        /// To fix this issue, specify a valid value:
        /// <code>
        /// enum UserType {
        ///   Administrator = 0x0ab
        /// }
        /// </code>
        /// </example>
        EnumValueInvalidHex = 10,

        /// <summary>
        /// The specified enum value has already been used by another enum member.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///   User = 10
        ///   Administrator = 10
        ///                   ^^
        /// }
        /// </code>
        ///
        /// To fix this issue, make sure the values are unique:
        /// <code>
        /// enum UserType {
        ///   User = 10
        ///   Administrator = 11
        /// }
        /// </code>
        /// </example>
        EnumValueDuplicated = 11,

        /// <summary>
        /// The specified namespace scope is not in the list of known namespaces.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// namespace notalang mynamespace
        ///           ^^^^^^^^
        /// </code>
        ///
        /// To fix this issue change the scope to a valid language target:
        /// <code>
        /// namespace csharp Thrift.Net.Examples
        /// </code>
        /// </example>
        NamespaceScopeUnknown = 100,

        /// <summary>
        /// A namespace has been specified without a scope.
        /// </summary>
        /// <example>
        /// The following examples produces this error:
        /// <code>
        /// namespace mynamespace
        /// </code>
        ///
        /// To fix this issue add a scope:
        /// <code>
        /// namespace csharp Thrift.Net.Examples
        /// </code>
        /// </example>
        NamespaceScopeMissing = 101,

        /// <summary>
        /// The namespace keyword has been specified, but without a scope or
        /// namespace being provided.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// namespace
        /// </code>
        ///
        /// To fix this issue provide a scope and namespace:
        /// <code>
        /// namespace csharp Thrift.Net.Examples
        /// </code>
        /// </example>
        NamespaceAndScopeMissing = 102,

        /// <summary>
        /// A namespace scope has been specified without a corresponding
        /// namespace being provided.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// namespace csharp
        /// </code>
        ///
        /// To fix this issue provide a namespace:
        /// <code>
        /// namespace csharp Thrift.Net.Examples
        /// </code>
        /// </example>
        NamespaceMissing = 103,

        /// <summary>
        /// The same namespace scope has been specified multiple times.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// namespace csharp Thrift.Net.Examples.A
        /// namespace csharp Thrift.Net.Examples.B
        ///           ^^^^^^
        /// </code>
        ///
        /// To fix this issue remove the duplicate declaration, or change its scope:
        /// <code>
        /// namespace csharp Namespace.A
        /// namespace netstd Namespace.B
        /// </code>
        /// </example>
        NamespaceScopeAlreadySpecified = 104,

        /// <summary>
        /// The namespace statement has been terminated with a list separator.
        /// </summary>
        /// <example>
        /// <code>
        /// namespace csharp Thrift.Net.Examples,
        ///                                     ^
        /// namespace netstd Thrift.Net.Examples;
        ///                                     ^
        /// </code>
        ///
        /// To fix this remove the separators:
        /// <code>
        /// namespace csharp Thrift.Net.Examples
        /// namespace netstd Thrift.Net.Examples
        /// </code>
        /// </example>
        NamespaceStatementTerminatedBySeparator = 105,

        /// <summary>
        /// A struct has been defined without a name.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// struct {
        /// ^^^^^^^^
        ///   0: i32 Id
        /// }
        /// </code>
        ///
        /// To fix this issue provide a name for the struct:
        /// <code>
        /// struct User {
        ///   0: i32 Id
        /// }
        /// </code>
        /// </example>
        StructMustHaveAName = 200,

        /// <summary>
        /// The specified field name has already been used in the same struct.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// struct User {
        ///   i32 Username
        ///   string Username
        ///          ^^^^^^^^
        /// }
        /// </code>
        ///
        /// To fix this issue, delete or rename one of the fields:
        /// <code>
        /// struct User {
        ///   i32 Id
        ///   string Username
        /// }
        /// </code>
        /// </example>
        StructFieldNameAlreadyDefined = 201,

        /// <summary>
        /// The specified field Id has already been defined in the same struct.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// struct User {
        ///   0: i32 Id,
        ///   0: string Username
        ///   ^
        /// }
        /// </code>
        ///
        /// To fix this issue, make sure all the field Ids are unique:
        /// <code>
        /// struct User {
        ///   0: i32 Id,
        ///   1: string Username
        /// }
        /// </code>
        /// </example>
        StructFieldIdAlreadyDefined = 202,

        /// <summary>
        /// The specified field Id is not a positive integer.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// struct User {
        ///   abc: i32 Id
        ///   ^^^
        ///   -1: string Username
        ///   ^^
        /// }
        /// </code>
        ///
        /// To fix this issue, make sure all the field Ids are positive integers:
        /// <code>
        /// struct User {
        ///   0: i32 Id
        ///   1: string Username
        /// }
        /// </code>
        /// </example>
        StructFieldIdMustBeAPositiveInteger = 203,

        /// <summary>
        /// The `slist` type is deprecated, and should no-longer be used. Please
        /// use the `string` type instead.
        /// </summary>
        /// <example>
        /// The following example produces this warning:
        /// <code>
        /// struct User {
        ///   slist Username
        ///   ^^^^^
        /// }
        /// </code>
        ///
        /// To fix this issue, change the type of the field from `slist` to `string`:
        /// <code>
        /// struct User {
        ///   string Username
        /// }
        /// </code>
        /// </example>
        SlistDeprecated = 204,

        /// <summary>
        /// A field has been specified without a field Id. This can lead to backwards
        /// incompatible changes being made to the protocol by accident.
        /// </summary>
        /// <example>
        /// The following example produces this warning:
        /// <code>
        /// struct User {
        ///   string Username
        /// }
        /// </code>
        ///
        /// This can cause a backwards incompatible change to be made if the field is moved
        /// or reordered. When no field Id is explicitly defined, the Thrift compiler
        /// automatically assigns negative field Ids starting at -1, as shown in the
        /// following example:
        ///
        /// <code>
        /// struct User {
        ///   i32 Id           // Field Id -1
        ///   string Username  // Field Id -2
        ///   string CreatedOn // Field Id -3
        /// }
        /// </code>
        ///
        /// So if the `Username` field was deleted it would cause `CreatedOn` to end up with
        /// field Id `-2`. Similarly, if the order of the two fields is swapped in the IDL,
        /// it would cause both fields to swap field Ids.
        ///
        /// To fix this issue, explicitly specify Ids for all fields:
        /// <code>
        /// struct User {
        ///   1: i32 Id
        ///   2: string Username
        ///   3: string CreatedOn
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// The official Thrift compiler doesn't allow negative field Ids to be
        /// specified in IDL files by default, but allows you to opt-in via the
        /// `--allow-neg-keys` flag. The Thrift.Net compiler will not support that
        /// behaviour unless it's asked for.
        /// </remarks>
        FieldIdNotSpecified = 205,

        /// <summary>
        /// A list has been declared, but no element type has been specified.
        /// </summary>
        /// <example>
        /// The following example produces this warning:
        /// <code>
        /// struct User {
        ///   1: list&lt;&gt; Emails
        ///          ^^
        /// }
        /// </code>
        ///
        /// To fix this issue, specify an element type:
        /// <code>
        /// struct User {
        ///   1: list&lt;string&gt; Emails
        /// }
        /// </code>
        /// </example>
        ListMustHaveElementTypeSpecified = 206,

        /// <summary>
        /// A set has been declared, but no element type has been specified.
        /// </summary>
        /// <example>
        /// The following example produces this warning:
        /// <code>
        /// struct User {
        ///   1: set&lt;&gt; Emails
        ///          ^^
        /// }
        /// </code>
        ///
        /// To fix this issue, specify an element type:
        /// <code>
        /// struct User {
        ///   1: set&lt;string&gt; Emails
        /// }
        /// </code>
        /// </example>
        SetMustHaveElementTypeSpecified = 207,

        /// <summary>
        /// A map has been declared, but no key type has been specified.
        /// </summary>
        /// <example>
        /// The following example produces this warning:
        /// <code>
        /// struct User {
        ///   1: map&lt;, string&gt; Emails
        ///          ^^^^^^^^
        /// }
        /// </code>
        ///
        /// To fix this issue, specify an element type:
        /// <code>
        /// struct User {
        ///   1: map&lt;EmailType, string&gt; Emails
        /// }
        /// </code>
        /// </example>
        MapMustHaveKeyTypeSpecified = 208,

        /// <summary>
        /// A map has been declared, but no value type has been specified.
        /// </summary>
        /// <example>
        /// The following example produces this warning:
        /// <code>
        /// struct User {
        ///   1: map&lt;EmailType, &gt; Emails
        ///          ^^^^^^^^^^^^^
        /// }
        /// </code>
        ///
        /// To fix this issue, specify an element type:
        /// <code>
        /// struct User {
        ///   1: map&lt;EmailType, string&gt; Emails
        /// }
        /// </code>
        /// </example>
        MapMustHaveValueTypeSpecified = 209,

        /// <summary>
        /// A map has been declared, but no key or value types have been specified.
        /// </summary>
        /// <example>
        /// The following example produces this warning:
        /// <code>
        /// struct User {
        ///   1: map&lt;&gt; One
        ///          ^^
        ///   2: map&lt;,&gt; Two
        ///          ^^^
        ///   2: map Three
        ///       ^^^
        /// }
        /// </code>
        ///
        /// To fix this issue, specify an element type:
        /// <code>
        /// struct User {
        ///   1: map&lt;EmailType, string&gt; Emails
        /// }
        /// </code>
        /// </example>
        MapMustHaveKeyAndValueTypeSpecified = 210,

        /// <summary>
        /// A syntax error has been reported by the Antlr parser.
        /// </summary>
        /// <example>
        /// Where possible we should aim to make the grammar loose enough to catch
        /// most errors ourselves, but this is a catch-all to make sure we report
        /// everything. Here's some examples:
        /// <code>
        /// structe User {} // The `struct` keyword has an `e` at the end
        /// struct User {
        ///
        /// ] // The closing brace is the wrong type
        /// </code>
        ///
        /// Since this is an unexpected error, we can't really say what to do to
        /// resolve it.
        /// </example>
        GenericParseError = 300,

        /// <summary>
        /// A field references a type that cannot be found.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// enum UserType {
        ///   User,
        ///   Administrator
        /// }
        ///
        /// struct User {
        ///   1: UserTyp Type
        ///      ^^^^^^^
        /// }
        /// </code>
        ///
        /// To resolve this issue, make sure there are no mistakes in the type name:
        /// <code>
        /// enum UserType {
        ///   User,
        ///   Administrator
        /// }
        ///
        /// struct User {
        ///   1: UserType Type
        /// }
        /// </code>
        /// </example>
        UnknownType = 400,

        /// <summary>
        /// A Thrift document has been declared with nothing other than imports
        /// and namespace declarations.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// namespace * Thrift.Net.Examples
        ///
        /// import "UserType.thrift"
        /// </code>
        ///
        /// To resolve this issue, delete the file, or add some Thrift definitions.
        /// </example>
        DocumentEmpty = 500,

        /// <summary>
        /// A field in a union has been marked as required. This doesn't make sense
        /// because unions only allow one of their fields to be set, meaning
        /// that all the fields are implicitly optional.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// union Request {
        ///   1: required string Query
        ///      ^^^^^^^^
        /// }
        /// </code>
        ///
        /// To resolve this issue, change the field to optional or remove the
        /// `required` modifier:
        /// <code>
        /// union Request {
        ///   1: optional string Query
        /// }
        /// </code>
        /// </example>
        UnionCannotContainRequiredFields = 600,

        /// <summary>
        /// A union has been declared with no name.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// union {
        ///   1: string Query
        /// }
        /// </code>
        ///
        /// To resolve this issue, add a name to the union:
        /// <code>
        /// union Request {
        ///   1: string Query
        /// }
        /// </code>
        /// </example>
        UnionMustHaveAName = 601,

        /// <summary>
        /// An exception has been declared with no name.
        /// </summary>
        /// <example>
        /// The following example produces this error:
        /// <code>
        /// exception {
        ///   1: string Id
        /// }
        /// </code>
        ///
        /// To resolve this issue, add a name to the exception:
        /// <code>
        /// exception NotFoundException {
        ///   1: string Id
        /// }
        /// </code>
        /// </example>
        ExceptionMustHaveAName = 700,
    }
}