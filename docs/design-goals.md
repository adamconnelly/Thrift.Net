# Design Goals

- Compiler should provide helpful error messages.
- Code generator that produces beautiful C# code.
- High performance.
- Built-in instrumentation.
- First-class C# citizen.
- vscode extension.
- Be an open project.
- Transparent release process.
- Provide linting and formatting support.

## Compiler

### Errors and Warnings

All the errors and warnings output by the compiler should include the following
components:

- Line number.
- Column information.
- Level (Error, Warning).
- Message.

In addition, where possible we should aim to provide suggestions on how to
resolve the problem.

## Code Generation

- Use a model + template approach for simplicity and to make changes easy.
- The generated code should follow C# standards and follow StyleCop guidelines.
- We should provide a _Tools_ package, similar to grpc.Tools, that can
  automatically generate C# files for any Thrift files in a project.

## Linting and Formatting

We should be able to hook into the compiler to provide linting and formatting.
We should provide a set of opinionated rules to help people write consistent
Thrift files, but allow these rules to be adjusted.

One example of this is the way that Thrift allows either `,` or `;` to be used
as a list separator, allowing you to write code like the following:

```thrift
enum Colours
{
    Red = 0,
    Blue = 1;
    Green = 2
}
```

We need the compiler to allow this since it's valid Thrift, but we should
recommend one or the other for consistency.

## First Class C# Citizen

The library should feel like a C# library to other developers, in terms of
naming, how the code is structured, etc. It shouldn't feel like a direct port
from another language. This means that we may take different decisions at
certain points from the official Apache implementation. But it also doesn't
prevent us from making the same decisions where it makes sense or is required.
