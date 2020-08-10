# Compilation

- [Explanation of the message format](#message-format).
- [Full list of messages](#messages).

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

Any Thrift compiler messages should be detailed in this section.

### TC0000 - Enum Must Have a Name

Reported when an enum is defined with no name specified, for example:

```thrift
enum {
  User,
  Administrator
}
```

To fix this add a name, for example:

```thrift
enum UserType {
  User,
  Administrator
}
```

### TC0001 - Enum Member Must Have a Name

An enum member has been defined without a name, for example:

```thrift
enum UserType {
  User = 0,
  = 1
}
```

To fix this add a name, for example:

```thrift
enum UserType {
  User = 0,
  Administrator = 1
}
```

### TC0002 - Enum Value Must Not Be Negative

An enum member has been specified with a negative value. For example:

```thrift
enum UserType {
  User = 0,
  Administrator = -1
}
```

To fix this use a positive value:

```thrift
enum UserType {
  User = 0,
  Administrator = 1
}
```

### TC0003 - Enum Value Must Be an Integer

An enum value has been specified that isn't an integer. For example:

```thrift
enum UserType {
  User = 0,
  Administrator = "hello"
}
```

To fix this change the value to an integer:

```thrift
enum UserType {
  User = 0,
  Administrator = 1
}
```

### TC0004 - Enum Value Must Be Specified

An enum member has been defined but the value is missing from the assign
expression. For example:

```thrift
enum UserType {
  User =
}
```

To fix this assign a value:

```thrift
enum UserType {
  User = 0
}
```

Alternatively, just remove the value expression completely:

```thrift
enum UserType {
  User
}
```

### TC0005 - Enum Member Equals Operator Missing

The equals operator is missing between an enum member and its value. For
example:

```thrift
enum UserType {
  User 1
}
```

To fix this issue add the missing equals operator:

```thrift
enum UserType {
  User = 1
}
```