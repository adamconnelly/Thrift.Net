# Add New Compiler Messages

This guide explains how to add a new compiler message.

## Identify Problem

First we need to identify the problem we're trying to catch. For example, maybe
we want to add an error if someone adds a namespace declaration without
specifying a scope. Here's a valid namespace declaration:

```thrift
namespace csharp MyNamespace
```

And here's an invalid declaration:

```thrift
namespace MyNamespace
```

## Adjust Grammar

Depending on the type of error you're trying to handle, you may need to adjust
the grammar to make it less strict. For example, if we have the following
definition for a namespace:

```antlr
namespaceDeclaration: 'namespace' scope=IDENTIFIER ns=IDENTIFIER;
```

It will fail to parse text with a missing scope because the scope is not
optional. To allow us to parse text with a missing scope, we can mark the scope
as optional using the `?` modifier:

```antlr
namespaceDeclaration: 'namespace' scope=IDENTIFIER? ns=IDENTIFIER;
```

This allows us to test for the presence or absence of the scope in our visitor
by checking if `context.scope == null`.

## Add a Test

The next thing to do is to add a test for our error. This lets us build up a
formal definition of how the compiler functions, as well as giving us confidence
that our code is working and we haven't broken any other rules.

We add our test to
`src/Thrift.Net.Tests/Compilation/ThriftCompiler/<element>ErrorTests.cs`, so in
our case `NamespaceErrorTests.cs`.

Our tests inherit from the `ThriftCompilerTests` base class, which provides some
helper methods for testing snippets of code for errors. In our case we want to
check for an error message, so we'd write the following test:

```csharp
[Fact]
public void Compile_ScopeMissing_ReportsError()
{
    this.AssertCompilerReturnsErrorMessage(
        "$namespace mynamespace$",
        CompilerMessageId.NamespaceScopeMissing);
}
```

This checks that the compiler returns the
`CompilerMessageId.NamespaceScopeMissing` error, and the `$` signs around the
code indicate the character positions we should output in any error message
produced. For example, the test we've just written would expect the following
error message:

```text
myfile.thrift(1,1-21): Error TC0007: A namespace scope must be specified [/home/adam/github.com/adamconnelly/thrift.net/thrift-samples/enum.thrift]
```

This means that the error was reported on line 1 from columns 1 - 21. If we just
wanted to highlight the `namespace` keyword as the problem then we could use the
following text instead:

```text
$namespace$ mynamespace
```

This would produce an error highlighting character positions between 1 and 9.

## Add New Message Id

Our compiler messages are all defined in
[src/Thrift.Net.Compilation/CompilerMessageId.cs](src/Thrift.Net.Compilation/CompilerMessageId.cs).
Add a new enum member for our new compiler error:

```csharp
...
NamespaceScopeUnknown = 100,
```

### Message Documentation

Each message in the enum includes documentation that includes an example of how
to produce the message, along with how to resolve the problem. Use the existing
enum members as a template.

## Create or Update the Binder

Each object that can be defined in the Thrift IDL file is represented by a
Symbol, and each Symbol has an associated Binder that can create the Symbol from
the Antlr parse tree. In our example, the Symbol is the
[Namespace](src/Thrift.Net.Compilation/Symbols/Namespace.cs) class, and the
Binder is the
[NamespaceBinder](src/Thrift.Net.Compilation/Binding/NamespaceBinder.cs).

Depending on your change, you may or may not need to adjust the Symbol and
Binder. For example, if the `Scope` property didn't already exist on the
`Namespace` object you would need to add it, and update the Binder to set the
scope when creating a `Namespace`.

For more information see the [binding](binding.md) documentation.

## Update the CompilationVisitor

Update the visitor to detect the problem and add a compiler message. In our
example we might end up adding something like the following:

```csharp
public override int? VisitNamespaceStatement(
            ThriftParser.NamespaceStatementContext context)
{
    var result = base.VisitNamespaceStatement(context);
    var @namespace = this.Document.FindSymbolForNode(context);

    if (@namespace.Scope == null)
    {
        // The namespace scope is missing. For example
        // `namespace mynamespace`
        this.AddError(
            CompilerMessageId.NamespaceScopeMissing,
            context.NAMESPACE().Symbol,
            context.ns);
    }

    return result;
}
```

## Add Text for the Message

We need to map our compiler Id to the human friendly text we're going to output.
To do that add your new message to
[src/Thrift.Net.Compilation/Resources/CompilerMessages.resx](src/Thrift.Net.Compilation/Resources/CompilerMessages.resx):

```xml
...
<data name="TC0100" xml:space="preserve">
    <value>The specified namespace scope is not valid</value>
</data>
<data name="TC0101" xml:space="preserve">
    <value>A namespace scope must be specified</value>
</data>
```

## Run Compiler

At this point you should be able to create an input file that produces your
error, and run the compiler:

```shell
$ npm run --silent compiler -- --input=./thrift-samples/enum.thrift --output-directory ./thrift-output
Starting compilation of enum.thrift
Compilation failed with 1 error(s):

thrift-samples/enum.thrift(1,1-28): Error TC0007: A namespace scope must be specified [/home/adam/github.com/adamconnelly/thrift.net/thrift-samples/enum.thrift]
```
