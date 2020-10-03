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
    Assert.Collection(
        result.Document.Namespaces,
        ns => Assert.Equal("Thrift.Net.Examples", ns.Name));
}
```

Run the test and check it fails. At this point we can move onto implementing the
functionality.

## Create Symbol, Builder and Binder

### Symbol

Symbols allow us to build a semantic model from the parse tree provided by Antlr
by augmenting it with additional logic that the compiler needs to perform things
like type resolution. Each Thrift object (structs, fields, enums, etc) have an
associated Symbol.

A Symbol is a C# class inheriting from `Symbol<TNode>`, where `TNode` is the
type of the Antlr tree node:

```csharp
public class Namespace : Symbol<NamespaceStatementContext>, INamespace
{
}
```

We also provide an interface for each symbol to facilitate unit testing.

The symbol class typically has a property for each piece of information we need
for analysis and code generation. In the case of the `Namespace`, this might be:

- `Scope` - the namespace scope, for example `csharp`, `netstd`, etc.
- `NamespaceName` - the namespace, for example `Thrift.Net.Examples`.
- `HasCSharpScope` - is the scope one that can be used for C# code generation?
- `HasKnownScope` - is the scope a valid Thrift scope.

**NOTE:** Symbols should be immutable - that is, any properties should be set
via the constructor, and should not have setters. Because of this, any child
symbols are created lazily since they need a reference to their parent.

### Binder

The Binder is the object responsible for creating the symbol from the Antlr
parse tree. A Binder inherits from `Binder<TNode, TSymbol, TParentSymbol>`,
where `TNode` is the Antlr node that the Binder binds, `TSymbol` is the type of
Symbol it produces, and `TParentSymbol` is the type of the Symbol's parent
(`Document` in the case of `Struct` and `Enum`, `Struct` in the case of `Field`,
etc):

```csharp
public class NamespaceBinder : Binder<NamespaceStatementContext, Namespace, IDocument>
{
}
```

A Binder implementation needs to override the `Bind()` method to perform its
work:

```csharp
/// <inheritdoc />
protected override Namespace Bind(NamespaceStatementContext node, IDocument parent)
{
    var builder = new NamespaceBuilder()
        .SetNode(node)
        .SetParent(parent)
        .SetBinderProvider(this.binderProvider)
        .SetScope(node.namespaceScope?.Text)
        .SetNamespaceName(node.ns?.Text);

    return builder.Build();
}
```

The `node` parameter contains information about the node in the tree that we're
Binding. In our example it will contain an `IDENTIFIER()` method that contains a
collection of identifiers (because our grammar rule specifies two identifiers
after the `namespace` keyword). We can adjust our grammar using _labels_ to make
it easier to grab pieces of information. For example:

```antlr
namespaceDeclaration: 'namespace' namespaceScope=IDENTIFIER ns=IDENTIFIER;
```

This allows us to access the two identifiers by name, as shown in the example
above.

### Builder

Builders are provided for all Symbols. A Builder is responsible for creating
(building) Symbol objects. The reason we have builders is because Symbols are
immutable. Having builders means that we don't need to update every place that
creates a Symbol when new parameters are added to that Symbol's constructor.
This is particularly important to avoid our tests being brittle and requiring
large numbers of tests to be updated every time an unrelated change is made to a
Symbol.

Builders inherit from `SymbolBuilder<TNode, TSymbol, TParentSymbol, TBuilder>`
where `TNode` is the type of Antlr node associated with the symbol, `TSymbol` is
the type of Symbol the builder creates, `TParentSymbol` is the type of the
parent symbol, and `TBuilder` is the type of the builder itself, used to
facilitate method chaining:

```csharp
public class NamespaceBuilder : SymbolBuilder<NamespaceStatementContext, Namespace, IDocument, NamespaceBuilder>
```

Builders have `SetXyz()` methods for each property on the Symbol:

```csharp
/// <summary>
/// Sets the scope.
/// </summary>
/// <param name="scope">The scope.</param>
/// <returns>The builder.</returns>
public NamespaceBuilder SetScope(string scope)
{
    this.Scope = scope;

    return this;
}
```

They also override a `Build()` method responsible for building the Symbol:

```csharp
/// <inheritdoc/>
public override Namespace Build()
{
    return new Namespace(
        this.Node,
        this.Parent,
        this.Scope,
        this.NamespaceName);
}
```

## Connect Symbol to Tree

At this point, you would update the `Document` Symbol to add the namespaces to
the Symbol tree. Because Symbols are immutable, the Document lazily creates its
child Symbols:

```csharp
/// <inheritdoc/>
public IReadOnlyCollection<Namespace> Namespaces
{
    get
    {
        return this.Node.header()?.namespaceStatement()
            .Select(namespaceNode => this.binderProvider
                .GetBinder(namespaceNode)
                .Bind<Namespace>(namespaceNode, this))
            .ToList();
    }
}
```

It may not be completely obvious because of the way that Antlr names its
generated methods, but `namespaceStatement()` in the example above actually
returns an array of `NamespaceStatementContext` objects. We can then get an
appropriate Binder via the `binderProvider`, and Bind it to a `Namespace`
symbol.

We can then add additional properties to the Document Symbol to make analysis
and code generation simpler. For example, here's a property that finds the valid
C# namespace to use from the set of namespace definitions in the document:

```csharp
/// <inheritdoc/>
public string CSharpNamespace
{
    get
    {
        var @namespace = this.Namespaces
            .LastOrDefault(n => n.HasCSharpScope);
        if (@namespace != null)
        {
            return @namespace.NamespaceName;
        }

        return this.Namespaces
            .FirstOrDefault(n => n.AppliesToAllTargets)?.NamespaceName;
    }
}
```

## Theories

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
    Assert.Equal(expected, result.Document.CSharpNamespace);
}
```

## Updating the Code Generator

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
    var input = "namespace csharp Thrift.Net.Tests";
    var compilationResult = this.Compiler.Compile(input.ToStream());

    // Act
    var output = this.Generator.Generate(compilationResult.Document);

    // Assert
    var (root, _, _) = ParseCSharp(output);
    var namespaceDeclaration = root.GetCompilationUnitRoot().Members.First() as NamespaceDeclarationSyntax;
    Assert.Equal("Thrift.Net.Tests", namespaceDeclaration.Name.ToString());
}
```

In that test, we use the Thrift compiler to create a `Document` to pass to the
generator, generate our code, and then parse it using the `ParseCSharp()`
method. This uses the [.NET Compiler Platform](https://github.com/dotnet/roslyn)
(AKA Roslyn) to parse our generated code.

Similar to the way that Thrift provides us with a Parse Tree of our Thrift
syntax that we can then analyse, Roslyn provides us with an Abstract Syntax Tree
(AST) that represents the C# we provide to it. This allows us to
programmatically verify that our generator produces valid C#.

## Run the Compiler

At this point you could run the application end to end to check it generates
your new piece of syntax correctly:

```shell
$ npm run --silent compiler -- --input=./thrift-samples/namespace.thrift --output-directory ./thrift-output
Starting compilation of namespace.thrift
Compilation succeeded with no errors or warnings!
```

You should be able to check the output of your compilation in
`./thrift-output/thrift-samples/namespace.cs`.
