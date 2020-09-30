# Thrift.Net

A fully .NET implementation of the Thrift compiler and runtime library.

## Contributing

- [Development environment](#development-environment).
- [Antlr](#antlr).
- [Development Guides](#development-guides).
- [Commit Messages](#commit-messages).
- [Design Goals](design-goals.md).
- [Roadmap](roadmap.md).
- [Compilation Docs](compilation.md).
- [Other Resources](#other-resources).

### Development Environment

To work on the project, you need the following tools installed:

- [.NET Core 3.1](https://dotnet.microsoft.com/download).
- [JDK](https://jdk.java.net/java-se-ri/11) (see note below).
- [node.js](https://nodejs.org/en/download/).

The following tools are also recommended:

- vscode / Visual Studio.
- [Antlr extension](https://marketplace.visualstudio.com/items?itemName=mike-lischke.vscode-antlr4)
  for vscode.

**NOTES:**

- The Java Development Kit is required in order to run the Antlr code generation
  tool at build time. There is no runtime dependency on Java.

### Getting Started

The first thing to do once you've cloned the repo and installed the required
development tools is to install any npm packages:

```shell
npm install
```

This will also configure our shared Git hooks using
[Husky](https://github.com/typicode/husky). At the moment Husky is configured to
lint and format markdown.

### Antlr

We use [Antlr](https://www.antlr.org/) to parse Thrift code. Antlr provides us
with a simple way of analysing the Thrift code and producing a Parse Tree that
we can then extract information from.

Our Antlr thrift grammar can be found in
[src/Thrift.Net.Antlr/Thrift.g4](src/Thrift.Net.Antlr/Thrift.g4).

The official Thrift grammar can be found
[here](https://thrift.apache.org/docs/idl). Our grammar doesn't exactly match
the grammar from Thrift because we need to make our grammar slightly less strict
in order to provide great errors and warnings.

The [Thrift.Net.Antlr.csproj](src/Thrift.Net.Antlr.csproj) file is configured to
automatically build the grammar, so all you need to do to get up and running is
make sure you have a valid install of the JDK, however you can follow the
instructions on the Antlr site if you want to install the tool globally.

#### Antlr vscode Extension

Once you have the vscode extension installed, you can debug your grammar and
view the parse tree generated from a particular input file by running the _Debug
ANTLR4 grammar_ task from the _Run_ section of vscode. The file to parse can be
configured via the [.vscode/launch.json](.vscode/launch.json) file.

### Development Guides

These guides explain how to work on particular parts of the code base:

- [Adding new syntax to the compiler](add-new-syntax-to-compiler.md).
- [Add new compiler messages](add-new-compiler-messages.md).

### Commit Messages

We're using [Conventional Commits](https://www.conventionalcommits.org/) to
define our commit message format. The project is configured with commitlint to
check messages are valid when you commit.

An example of a valid commit message is the following:

```text
feat(compiler): add ability to parse an enum

- Did x because of y.
- Did z because of a.

Fixes #123
```

All commit messages must have a valid subject line and reference an issue. The
body of the commit message is optional.

### Running Apache Thrift Compiler via Docker

There's a Docker image available for the Thrift compiler that can be useful if
you need to check what the official compiler produces. Here's an example
`docker run` command:

```shell
docker run -v "$PWD:/data" thrift thrift -o /data/thrift-output --gen netstd /data/thrift-samples/structs/User.thrift
```

### Other Resources

- The [Roslyn Overview](https://github.com/dotnet/roslyn/wiki/Roslyn%20Overview)
  provides a really good high level overview of how compilers and syntax
  analysis works. We can use it for inspiration.
