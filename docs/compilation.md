# Compilation

- [Compilation Overview](#compilation-overview).
- [Explanation of the message format](#message-format).
- [Full list of messages](#messages).
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
