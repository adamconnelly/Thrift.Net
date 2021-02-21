# Compilation

- [Compilation Overview](#compilation-overview).
- [Explanation of the message format](#message-format).
- [Full list of messages](#messages).
- [Type System](#type-system).
- [Differences to official compiler](#differences-to-official-compiler).

## Compilation Overview

Compilation is made up of a number of stages:

- Parsing the input text into a syntax tree.
- [Binding](binding.md) the nodes in the tree into Symbols.
- Performing analysis and reporting any errors or warnings.
- Code generation.

## Message Format

Messages output by the compiler are in the following format:

```text
<relative-filename>(<line-number>,<start-position>[-<end-position>]): <type> <message-id>: <message> [<absolute-path>]
```

### relative-filename

`relative-filename` is the path to the file from the current working directory.
For example `src/users/UserType.thrift`.

### line-number

`line-number` is the line in the source file that the message applies to. Line
numbers start at 1.

### start-position

`start-position` is the character position in the line that has an error.
Character positions start at 1. For example, in the following text we would use
a start position of 6 if we want to highlight `1MyEnum` as an invalid
identifier.

```text
enum 1MyEnum {}
     ^^^^^^^
     6     12
```

### end-position

`end-position` is the end of the character range in the line that has an error.
Character positions start at 1. For example, in the following text we would use
an end position of 12 if we want to highlight `1MyEnum` as an invalid
identifier.

```text
enum 1MyEnum {}
     ^^^^^^^
     6     12
```

`end-position` is optional if it doesn't make sense to provide a range.

### type

The type must be one of:

- Warning - a potential problem that should be addressed, but which doesn't
  prevent compilation from succeeding.
- Error - a problem that must be addressed, preventing compilation from
  succeeding.

### message-id

A unique identifier for the message. All Thrift compiler messages are prefixed
with `TC`. See [messages](#messages) for the full list of messages.

### message

A human friendly message describing the problem. When adding new messages, aim
to make them as simple to understand as possible. For example, if we have the
following IDL:

```thrift
enum {
  One,
  Two
}
```

If we want to explain that the enum name is missing, instead of providing an
obscure message like:

> Identifier expected but was not found.

We should aim to provide a message like:

> An enum name must be specified.

### absolute-path

The absolute path to the thrift file that has an error. For example:

```text
/home/adam/github.com/adamconnelly/Thrift.Net/thrift-samples/enum.thrift
```

## Messages

The full list of compilation messages can be found in the
[CompilerMessageId](/src/Thrift.Net.Compilation/CompilerMessageId.cs) enum.

## Type System

The Thrift type system is made up of the following components:

- [Base Types](#base-types).
- [User Defined Types](#user-defined-types).
- [Collection Types](#collection-types).

### Base Types

Base Types are the lowest level building blocks that can be used to create
messages or services. They map to the primitive types of the various Thrift
target languages. The following table details the available base types, along
with their associated C# type:

| Base Type | C# Type  | Notes                                                       |
| --------- | -------- | ----------------------------------------------------------- |
| `bool`    | `bool`   |                                                             |
| `byte`    | `sbyte`  | `byte` is actually signed, and is equivalent to `i8`.       |
| `i8`      | `sbyte`  |                                                             |
| `i16`     | `short`  |                                                             |
| `i32`     | `int`    |                                                             |
| `i64`     | `long`   |                                                             |
| `double`  | `double` |                                                             |
| `string`  | `string` |                                                             |
| `binary`  | `byte[]` |                                                             |
| `slist`   | `string` | `slist` is deprecated, and `string` should be used instead. |

#### Notes on `bool`

Thrift supports the following constant expressions when assigning a value to
constants or a default value to fields:

- `true`.
- `false`.
- An integer expression where `<= 0` is `false` and positive numbers are true.

This can be seen in the following example:

```thrift
const bool TrueConstant = true  // Has a value of `true`
const bool FalseContant = false  // Has a value of `false`
const bool NegativeConstant = -2 // Has a value of `false`
const bool ZeroConstant = 0      // Has a value of `false`
const bool PositiveConstant = 1  // Has a value of `true`
```

### User Defined Types

Thrift supports the following user-defined types:

- [Structs](#structs).
- [Unions](#unions).
- [Exceptions](#exceptions).
- [Enums](#enums).
- [Typedefs](#typedefs).

#### Structs

Structs allow you to create complex messages by combining multiple fields into a
named type. Each field in a struct has a name and a Field Id, and both must be
unique within the struct. Structs map directly to classes in C#.

The following shows an example of a struct:

```thrift
struct User {
  1: i32 Id
  2: string Username
  3: UserType Type
}
```

#### Unions

Unions are identical to structs, except that only a single field in a union can
be set at a time.

#### Exceptions

Exceptions look almost identical to Thrift structs, but are used to create
exceptions in the target language, and to allow those exceptions to be thrown
from a Service and handled by the Thrift client. The following is an example of
an exception:

```thrift
exception NotFoundException {
  1: string Message
}
```

#### Enums

Enums represent enumerated types mapping names to values. The member names and
values must be unique within the enum. Enums map directly to enums in C#. The
following is an example of an enum:

```thrift
enum UserType {
  User = 1
  Administrator = 2
}
```

#### Typedefs

Typedefs allow you to create an alternative name for a type. The following is an
example of typedefs:

```thrift
typedef i8 tinyint
typedef list<string> stringlist
```

**NOTES:**

- The Thrift specification only allows base types and collection types to be
  specified in a typedef.

### Type Hierarchy

The type hierarchy in Thrift.Net looks like this:

```text
Type
  --> BaseType
  --> Struct
  --> Union
  --> Enum
  --> Exception
  --> List
  --> Set
  --> Map
```

#### Type vs Type Reference

A Type represents the definition of a Thrift type, and a Type Reference
represents the usage of that type. For example, in the following thrift
definition we're defining two types, `UserType` and `User`, with each field
containing a reference to a type:

```thrift
enum UserType {
  User = 1
  Administrator = 2
}

struct User {
  1: i32 Id
  2: string Username
  3: UserType Type
}
```

## Differences to Official Compiler

This section documents any deliberate decisions we have taken to vary the
functionality from the way the official compiler works.

### Negative and Zero Field Ids

The official compiler disallows negative or zero field Ids, meaning that the
following example is invalid:

```thrift
struct User {
  0: string Name
  -1: string Email
}
```

The official compiler outputs a warning for this because it uses negative field
Ids when no field Id is specified, but it still generates the code, and just
ignores the field Ids that have been specified.

We are increasing this to the level of an error since it ends up producing code
that doesn't match the original Thrift IDL.

Here's an example of the message from the official compiler:

```shell
[WARNING:/data/thrift-samples/structs/User.thrift:9] Nonpositive value (0) not allowed as a field key.

[WARNING:/data/thrift-samples/structs/User.thrift:10] No field key specified for Line1, resulting protocol may have conflicts or not be backwards compatible!
```

Here's an example of the Thrift.Net message:

```shell
thrift-samples/structs/User.thrift(9,5-5): Error TC0203: Field Id '0' is not valid. Please use a positive number. [/home/adam/github.com/adamconnelly/thrift.net/thrift-samples/structs/User.thrift]
```

**NOTE:** the official compiler supports a command-line argument to allow
non-positive field Ids. At the moment we are taking the decision not to support
this option unless someone requests it.

### Binary Constants

The official compiler allows you to declare a constant using the `binary` type,
but it's not clear how a binary field could be initialised based on the
available constant expressions defined in the Thrift IDL.

The official compiler will allow a string literal to be assigned to a binary
constant:

```thrift
const binary Blob = "testing123"
```

But the resulting C# code doesn't compile:

```csharp
public const byte[] Blob = "testing123";
```

As a result we've taken the decision to output an error message if a constant is
declared using the binary type:

```shell
Error TC0804: 'Blob' has been declared using the `binary` type, which is not supported for constants. Please change the type of your constant.
```

This should highlight it as a problem if anyone is actually using binary
constants, and we can then implement it properly. In the meantime it should
prevent code being generated that can't be compiled.
