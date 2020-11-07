# Contributing to Thrift.Net

Firstly, thanks a lot for taking the time to contribute. It means a lot and is
definitely appreciated!

The following is a set of guidelines on how to contribute to Thrift.Net. If you
notice any mistakes, or feel like there's additional information that would be
useful to other contributors, please feel free to raise a pull request for the
change.

## Table of Contents

- [How Can I Contribute?](#how-can-i-contribute)
  - [Making Feature Requests](#making-feature-requests)
  - [Reporting Bugs](#reporting-bugs)
  - [Pull Requests](#pull-requests)
- [Branches](#branches)
- [Getting Started](#getting-started)
  - [Development environment](#development-environment)
- [Installing NPM Packages](#installing-npm-packages)
- [Antlr](#antlr)
- [NPM Scripts](#npm-scripts)
- [Development Guides](#development-guides)
- [Code Style](#code-style)
- [Commit Messages](#commit-messages)
- [Compilation Docs](compilation.md)
- [Benchmarking](benchmarking.md)
- [Testing](testing.md)
- [Issues](#issues)
  - [Labels](#labels)
- [Build Pipelines](#build-pipelines)
- [Other Resources](#other-resources)

## How Can I Contribute

### Making Feature Requests

At the moment, while working towards an initial stable release, we need to keep
very focused. Because of this we may have to hold off considering feature
requests until the project is ready for production. We still welcome project
suggestions, but please understand that it may be some time before we can
investigate them fully.

To raise a feature request, please create a GitHub issue.

### Reporting Bugs

To report bugs with the project, please create a GitHub issue. Please bear in
mind that the project is very much a work in progress at the moment, so we
expect to find problems and rough edges.

When reporting a bug, if you can provide a unit test to reproduce the problem it
will make it much easier for someone to fix the bug. If you manage to create a
unit test for your problem, why not go one step further and create a
[pull request](#pull-requests) to fix it! :smile:

### Pull Requests

We follow the standard GitHub process for accepting contributions via Pull
Requests. When making a change, please make sure you follow our
[code style](#code-style), and write your commit messages according to our
[commit message](#commit-messages) guide.

During the review process, your reviewer may ask you to make changes to your
contribution. In general we ask that you push any changes made as a result of
feedback as separate commits, and avoid rebasing until the request has been
approved. This makes it easier for reviewers to view the changes you have made
as a result of feedback.

Where possible we prefer smaller, more focused commits to make sure that changes
are easier to review. This isn't always possible, but any attempts you can make
to keep your changes small and focused will definitely be appreciated!

## Branches

The main development branch for this project is called `main`. Any pull requests
should target the `main` branch.

## Getting Started

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

### Installing NPM Packages

We use various npm tools as part of our build and development tooling. The first
thing to do once you've cloned the repo and installed the required development
tools is to install any npm packages:

```shell
npm install
```

This will also configure our shared Git hooks using
[Husky](https://github.com/typicode/husky). At the moment Husky is configured to
lint and format markdown.

## Antlr

We use [Antlr](https://www.antlr.org/) to parse Thrift code. Antlr provides us
with a simple way of analysing the Thrift code and producing a Parse Tree that
we can then extract information from.

Our Antlr thrift grammar can be found in
[src/Thrift.Net.Antlr/Thrift.g4](/src/Thrift.Net.Antlr/Thrift.g4).

The official Thrift grammar can be found
[here](https://thrift.apache.org/docs/idl). Our grammar doesn't exactly match
the grammar from Thrift because we need to make our grammar slightly less strict
in order to provide great errors and warnings.

The [Thrift.Net.Antlr.csproj](/src/Thrift.Net.Antlr/Thrift.Net.Antlr.csproj)
file is configured to automatically build the grammar, so all you need to do to
get up and running is make sure you have a valid install of the JDK, however you
can follow the instructions on the Antlr site if you want to install the tool
globally.

### Antlr vscode Extension

Once you have the vscode extension installed, you can debug your grammar and
view the parse tree generated from a particular input file by running the _Debug
ANTLR4 grammar_ task from the _Run_ section of vscode. The file to parse can be
configured via the [.vscode/launch.json](/.vscode/launch.json) file.

## NPM Scripts

We provide the following npm scripts to simplify certain development tasks:

- `npm test` - runs all our unit tests.
- `npm run lint:markdown` - lints our markdown files.
- `npm run format:markdown` - formats our markdown files using prettier.
- `npm run antlr:grun` - runs the Antlr test harness.
- `npm run compiler -- <parameters>` - builds and runs the thrift compiler. See
  [below](#running-the-compiler) for more details.
- `npm run publish:compiler:linux-x64` - builds the compiler executable for
  Linux, and outputs it into the `artifacts` directory.
- `npm run publish:compiler:win-x64` - builds the compiler executable for
  Windows, and outputs it into the `artifacts` directory.

### Running the Compiler

You can use `npm run compiler` to build and run the Thrift compiler. You should
pass any arguments after the `--` flag. Run `npm run compiler -- --help` to
output the help message for the compiler.

## Development Guides

These guides explain how to work on particular parts of the code base:

- [Adding new syntax to the compiler](add-new-syntax-to-compiler.md).
- [Add new compiler messages](add-new-compiler-messages.md).

## Code Style

<!-- markdownlint-disable MD026 -->

### C&#35;

<!-- markdownlint-enable MD026 -->

We use [StyleCop](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) to make
sure our C# code conforms to a common standard, and is well documented. StyleCop
is configured to automatically run as part of the C# build process, and if you
use an editor like vscode or Visual Studio, any StyleCop violations should be
highlighted for you.

In general we stick to the standard StyleCop rules, unless there is a compelling
reason not to. One major exception to this is in the unit tests project. Many of
the StyleCop rules are disabled for our tests because they don't form part of
the public API of the project.

### Markdown

We use [markdownlint](https://github.com/DavidAnson/markdownlint) to lint our
markdown files, and we use [prettier](https://prettier.io/) to automatically
format our markdown files when committing via Husky.

## Commit Messages

We use [Conventional Commits](https://www.conventionalcommits.org/) to define
our commit message format. The project is configured with commitlint to check
messages are valid when you commit.

An example of a valid commit message is the following:

```text
feat(compiler): add ability to parse an enum

- Did x because of y.
- Did z because of a.

Fixes #123
```

All commit messages must have a valid subject line and reference an issue. The
body of the commit message is optional.

## Running Apache Thrift Compiler via Docker

There's a Docker image available for the Thrift compiler that can be useful if
you need to check what the official compiler produces. Here's an example
`docker run` command:

```shell
docker run -v "$PWD:/data" jaegertracing/thrift:0.13 thrift -o /data/thrift-output --gen netstd /data/thrift-samples/structs/User.thrift
```

## Issues

### Labels

We use the following labels to organise issues. All of our labels are in the
format `<type>/<value>`, for example `type/epic`.

#### `type/<issue-type>`

We support the following types of issue:

- bug - used to report a bug in Thrift.Net.
- feature-request - used to ask for new functionality.
- epic - indicates a major piece of work that will need to worked on in multiple
  pieces.

#### `component/<component>`

The `component` label is used to indicate the part of the project affected / the
part of the project that the issue relates to. We have the following components:

- benchmarks - anything relating to benchmarking or performance analysis.
- compiler - anthing relating to the Thrift compiler itself.
- docs - anything relating to our documentation.
- testing - anything relating to testing.

## Build Pipelines

Our build pipelines can be found in the [pipelines](/pipelines) folder. We use
Azure Pipelines for CI/CD, and you can view the configured pipelines
[here](https://dev.azure.com/adamrpconnelly/Thrift.Net/_build).

To edit an existing pipeline, just make a change to the pipeline's yaml
definition and follow the standard PR process. If you need a new pipeline to be
added, please discuss it in an issue so that someone with the correct
permissions can set it up for you.

## Other Resources

- The [Roslyn Overview](https://github.com/dotnet/roslyn/wiki/Roslyn%20Overview)
  provides a really good high level overview of how compilers and syntax
  analysis works. We can use it for inspiration.
