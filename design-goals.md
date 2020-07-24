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

## Rough Project Structure

- ThriftSharp.Antlr - contains grammar files and hosts the generated code.
- ThriftSharp - contains the main runtime.
- ThriftSharp.Compiler - console app that can be used to compile Thrift code.
- ThriftSharp.Tools - adds codegen support to a C# project.
- ThriftSharp.Prometheus - adds Prometheus instrumentation.

The idea should be that all you need to use Thrift should be `ThriftSharp`. If
you want to add Prometheus metrics, you can opt-in to `ThriftSharp.Prometheus`.
The core runtime will provide hooks that the Prometheus library can use to provide
instrumentation.

## Compiler

### Errors and Warnings

All the errors and warnings output by the compiler should include the following
components:

- Line number.
- Column information.
- Level (Error, Warning).
- Message.

In addition, where possible we should aim to provide suggestions on how to resolve
the problem.

## Code Generation

- Use a model + template approach for simplicity and to make changes easy.
- The generated code should follow C# standards and follow StyleCop guidelines.
- We should provide a _Tools_ package, similar to grpc.Tools, that can automatically
  generate C# files for any Thrift files in a project.

## Linting and Formatting

We should be able to hook into the compiler to provide linting and formatting.
We should provide a set of opinionated rules to help people write consistent Thrift
files, but allow these rules to be adjusted.

One example of this is the way that Thrift allows either `,` or `;` to be used as
a list separator, allowing you to write code like the following:

```thrift
enum Colours
{
    Red = 0,
    Blue = 1;
    Green = 2
}
```

We need the compiler to allow this since it's valid Thrift, but we should recommend
one or the other for consistency.

## First Class C# Citizen

I want the library to feel like a C# library to other developers, in terms of naming,
how the code is structured, etc. I don't want it to feel like a direct port from
another language.
