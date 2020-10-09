# Roadmap

We are currently working towards an initial stable release of the project. This
is tracked on the
[Initial Stable Release](https://github.com/adamconnelly/Thrift.Net/projects/1)
project board, and consists of these main high-level milestones:

1. [Thrift compilation](#thrift-compilation).
2. [Tools project](#tools-project) to automate compilation.
3. [Runtime library](#runtime-library).
4. [Instrumentation](#instrumentation).
5. [Linting and formatting](#linting-and-formatting).

## Thrift Compilation

At this stage we'll just aim to produce code that's compatible with Apache's
Thrift library. The compiler should report any errors sensibly, and should be
able to generate any thrift code.

## Tools Project

At this stage we'll create a Tools project, similar to grpc.Tools, that is able
to automatically compile any thrift IDL files when added to a C# project.

## Runtime Library

At this stage we'll implement a new Thrift .NET runtime library. This will
remove any dependency on Apache's implementation, and will also allow us to
alter the generated code to suit ourselves if necessary.

This will also include adding the code necessary to make it easy to implement
Thrift services in ASP.NET and ASP.NET Core.

## Instrumentation

At this stage we'll provide points that people can hook into for logging and
metrics, and provide a library that add Prometheus metrics out the box.

## Linting and Formatting

At this stage we'll define a style guide, and extend the Thrift compiler to
analyse the code for style violations, and provide the ability to automatically
fix them.
