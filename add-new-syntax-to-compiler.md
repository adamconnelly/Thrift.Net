# Adding a New Piece of Syntax to the Compiler

This file gives an overview of how to go about implementing a new compiler
feature.

## Adding to the Compiler

This section provides a quick overview of adding a new piece of syntax to the
compiler. We'll take the example of parsing a namespace declaration:

```thrift
namespace * Thrift.Net.Examples
```

### Updating the Grammar

The first step is to update the grammar so that Antlr can parse the piece of
syntax you are adding. Use the
[Thrift IDL spec](https://thrift.apache.org/docs/idl) as a guide, and update the
[src/Thrift.Net.Antlr/Thrift.g4](src/Thrift.Net.Antlr/Thrift.g4) file. To begin
with it might look something like this:

```antlr
namespaceDeclaration: 'namespace' IDENTIFIER IDENTIFIER;
```

That is, a namespace declaration is made up of the `namespace` keyword, followed
by two identifiers. At this stage, save the grammar file and run the ANTLR4
Debug task (defined in the [launch.json](.vscode/launch.json) file) with a
sample file containing a single namespace declaration to view the tree produced.
It should look something like this:

![Antlr Debug Example](Antlr-Debug-Example.png)

At this point, if it looks correct continue, otherwise make changes to the
grammar until you can successfully parse your new syntax.

### Adding a Test

The next step is to add a unit test to check that you can parse the new syntax
correctly.

All of the tests for compilation can be found under
[src/Thrift.Net.Tests/Compilation/ThriftCompiler]. Each grammar element we want
to parse is split into (at least) two separate test files:

- `<Name>ParsingTests.cs` - checks that we can parse the correct information
  from a code snippet and generate the correct model.
- `<Name>ErrorTests.cs` - checks that various errors in the code produce the
  correct compiler messages.

For now we're just going to focus on parsing, and will ignore errors, so we'll
create a `NamespaceParsingTests.cs` file. You might want to start by just
checking that if a namespace is provided, it's extracted from the input and used
to populate the `ThriftDocument` object returned:

```csharp
[Fact]
public void Compile_SetsNamespace()
{
    // Arrange
    var compiler = new ThriftCompiler();
    var input = "namespace netcore Thrift.Net.Examples";

    // Act
    var result = compiler.Compile(input.ToStream());

    // Assert
    Assert.Equal("Thrift.Net.Examples", result.Document.Namespace);
}
```

Run the test and check it fails. At this point we can move onto implementing the
functionality.

## Update Compilation Visitor

At the moment, most of the parsing work is performed in
[src/Thrift.Net.Compilation/CompilationVisitor.cs](src/Thrift.Net.Compilation/CompilationVisitor.cs).

The `CompilationVisitor` is an Antlr Parse Tree Visitor. This uses the
[visitor pattern](https://en.wikipedia.org/wiki/Visitor_pattern) to allow us to
perform actions as Antlr walks down the parse tree. Antlr automatically
generates a `Visit<grammar-rule-name>()` method for each rule defined in our
grammar. We can override these methods to provide a hook that we can use to
extract information from our source grammar.

For example, if we want to hook into our `namespaceDeclaration` rule, we would
add the following override:

```csharp
public override int? VisitNamespaceStatement(
    ThriftParser.NamespaceStatementContext context)
{
    return base.VisitNamespaceStatement(context);
}
```

You'll notice that the method returns an `int?`. This is simply because Antlr
forces you to set a return type for your visitor methods. Currently this is
completely unused.

The `context` parameter contains information about the node in the tree that
we're visiting. In our example it will contain an `IDENTIFIER()` method that
contains a collection of identifiers (because our grammar rule specifies two
identifiers after the `namespace` keyword). We can adjust our grammar using
_labels_ to make it easier to grab pieces of information. For example:

```antlr
namespaceDeclaration: 'namespace' scope=IDENTIFIER ns=IDENTIFIER;
```

This allows us to access the two identifiers by name:

```csharp
public override int? VisitNamespaceStatement(
    ThriftParser.NamespaceStatementContext context)
{
    var scope = context.scope;
    var namespaceName = context.ns;

    // Do something with this information

    return base.VisitNamespaceStatement(context);
}
```

At the moment what we do is store information like this as a property on the
visitor, which then allows the
[ThriftCompiler](src/Thrift.Net.Compilation/ThriftCompiler.cs) to extract the
information and create a `ThriftDocument` that can be used for code generation:

```csharp
visitor.Visit(document);

var namespaceName = visitor.Namespace;

// Do something with the namespace
```

### Theories

The [xunit](https://xunit.net/) library we're using for unit testing provides
the ability to pass multiple test cases to a test. This can be useful in
situations like this where we want to check that our compiler can handle various
sets of input:

```csharp
[Theory]
[InlineData("namespace * Thrift.Net.Examples", "Thrift.Net.Examples")]
[InlineData("namespace delphi Thrift.Net.Examples", null)]
[InlineData("namespace csharp Thrift.Net.Examples", "Thrift.Net.Examples")]
[InlineData("namespace netcore Thrift.Net.Examples", "Thrift.Net.Examples")]
public void Compile_SetsNamespace(string input, string expected)
{
    // Arrange
    var compiler = new ThriftCompiler();

    // Act
    var result = compiler.Compile(input.ToStream());

    // Assert
    Assert.Equal(expected, result.Document.Namespace);
}
```

### Updating the Code Generator

Once you can successfully compile your code, you need to update the code
generator to create some output. The code generator can be found at
[src/Thrift.Net.Compilation/ThriftDocumentGenerator.cs](src/Thrift.Net.Compilation/ThriftDocumentGenerator.cs),
and the template for generating C# from the `ThriftDocument` model object can be
found at [src/Thrift.Net.Compilation/Templates/csharp.stg]. The template uses
[StringTemplate](https://www.stringtemplate.org/), which is the code generation
library that goes along with Antlr.

Similarly to parsing, you can write tests for the code generator. An example
test to check that the namespace is generated correctly might look like this:

```csharp
[Fact]
public void Generate_NamespaceSupplied_SetsCorrectNamespace()
{
    // Arrange
    var generator = new ThriftDocumentGenerator();
    var document = new ThriftDocument("Thrift.Net.Tests", new List<EnumDefinition>());

    // Act
    var output = generator.Generate(document);

    // Assert
    var root = ParseCSharp(output);
    var namespaceDeclaration = root.Members.First() as NamespaceDeclarationSyntax;
    Assert.Equal("Thrift.Net.Tests", namespaceDeclaration.Name.ToString());
}
```

In that test, we create a `ThriftDocument` to pass to the generator, generate
our code, and then parse it using the `ParseCSharp()` method. This uses the
[.NET Compiler Platform](https://github.com/dotnet/roslyn) (AKA Roslyn) to parse
our generated code. Similar to the way that Thrift provides us with a Parse Tree
of our Thrift syntax that we can then analyse, Roslyn provides us with an
Abstract Syntax Tree (AST) that represents the C# we provide to it. This allows
us to programmatically verify that our generator produces valid C#.

**NOTE:** since we're using Roslyn anyway for testing, it might actually make
sense to use it to generate our C# code instead of StringTemplate. We'll just
need to weigh up the benefits against whether it makes the code more complex and
less maintainable.

## Run the Compiler

At this point you could run the application end to end to check it generates
your new piece of syntax correctly:

```shell
$ npm run --silent compiler -- --input=./thrift-samples/namespace.thrift --output-directory ./thrift-output
Starting compilation of namespace.thrift
Compilation succeeded with no errors or warnings!
```

You should be able to check the output of your compilation in
`./thrift-output/thirft-samples/namespace.cs`.
