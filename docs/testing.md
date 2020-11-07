# Testing

This page explains the testing strategy in place for Thrift.Net. Our strategy is
driven by some of the key goals of the project:

- The ability to release new functionality and fixes as soon as they are ready.
- Welcoming contributions from anyone who wants to help with the project.

In order to achieve these goals while being confident that any changes don't
break existing functionality, and that we maintain a high level of quality, we
need to have an automated testing strategy in place.

To achieve this we use a combination of different types of testing:

- [Unit testing](#unit-testing).
- [End to end testing](#end-to-end-testing)

## Unit Testing

All of our unit tests can be found in the
[Thrift.Net.Tests](/src/Thrift.Net.Tests) project. Within this project, the
tests are arranged based on the project that they are testing. For example:

- _Compilation_ has tests for the _Thrift.Net.Compilation_ project.
- _Compiler_ has tests for the _Thrift.Net.Compiler_ project.

We use the following tools for unit testing:

- [xUnit.net](https://xunit.net/) as our unit test framework.
- [NSubstitute](https://nsubstitute.github.io/) for mocking/substituting
  dependencies.
- [Coverlet](https://github.com/coverlet-coverage/coverlet) for code coverage.

The following sections provide more information about our unit testing
guidelines:

- [AAA](#aaa)
- [One Test - One Assertion](#one-test---one-assertion)
- [Code Coverage](#code-coverage)
- [Testing Language Features](#testing-language-features)
- [Testing Compiler Message](#testing-compiler-messages)
- [XUnit Theories](#xunit-theories)

### AAA

Tests should typically follow the _Arrange-Act-Assert_ (AAA) pattern. This is
designed to keep tests simple and focused by splitting them into three separate
sections:

```csharp
[Fact]
public void DoesSomething()
{
    // Arrange
    var calculator = new Calculator();

    // Act
    var result = calculator.Add(1, 2);

    // Assert
    Assert.Equal(3, result);
}
```

As shown above, the _Arrange_ section performs any setup required for the test,
the _Act_ section performs the action we want to test, and the _Assert_ section
performs any assertions.

### Test Naming

Our tests use the following naming scheme:

```text
<ItemBeingTested>_<Scenario>_<Expectation>
```

For example, if we had a test for the `StructBinder` that ensured a struct's
name was set if it was supplied, it might be called
`Bind_NameSupplied_SetsName()`.

Sometimes we want to split tests up to avoid a single test class becoming
unmanageable. For example, we might end up with a structure like this:

```text
EnumMemberBinder
  --> GetEnumValueTests.cs
  --> IsValueImplicitTests.cs
  --> NameTests.cs
  --> ValueTests.cs
```

In this case, we can drop the _ItemBeingTested_ part of the name, because it's
implied by the filename. So some example test names in `GetEnumValueTests` are:

- `NodeIsOnlyMember_ReturnsZero()`.
- `HasPreviousSibling_ReturnsNextValue()`.
- `PreviousSiblingHasCustomValue_ReturnsNextValue()`.

### One Test - One Assertion

In general, a single test method should have a single assertion. If you need to
check multiple things, it probably means the test needs to be split up into
multiple tests.

```csharp
// Good: we have two separate tests to test separate scenarios
[Fact]
public void Compile_DocumentContainsStruct_AddsStructToModel()
{
    // Arrange
    var input = "struct User {}";

    // Act
    var result = this.compiler.Compile(input.ToStream());

    // Assert
    Assert.Collection(
        result.Document.Structs,
        item => Assert.Equal("User", item.Name));
}

[Fact]
public void Compile_StructContainsFields_AddsFieldsToStruct()
{
    // Arrange
    var input =
@"struct User {
  i32 Id
  string Name
}";

    // Act
    var result = this.compiler.Compile(input.ToStream());

    // Assert
    var definition = result.Document.Structs.First();

    Assert.Collection(
        definition.Fields,
        item => Assert.Equal("Id", item.Name),
        item => Assert.Equal("Name", item.Name));
}

// Bad: we're trying to test at least three different things in a single test
[Fact]
public void Compile_DocumentContainsStruct_ParsesStruct()
{
    // Arrange
    var input =
@"struct User {
  1: i32 Id
  5: string Name
}";

    // Act
    var result = this.compiler.Compile(input.ToStream());

    // Assert
    var definition = result.Document.Structs.First();

    // 1. We're testing the struct name is parsed correctly here
    Assert.Equal("User", definition.Name);
    Assert.Collection(
        definition.Fields,
        item =>
        {
            // 2. We're testing field Ids are parsed correctly here
            Assert.Equal(1, item.FieldId);
            // 3. We're testing field names are parsed correctly here
            Assert.Equal("Id", item.Name);
        },
        item =>
        {
            Assert.Equal(5, item.FieldId);
            Assert.Equal("Name", item.Name);
        });
}
```

At the same time, there are definitely situations where this doesn't make sense,
so this is meant as a guide rather than a hard and fast rule.

### Code Coverage

We generate code coverage reports during the build process in
[Azure Pipelines](https://dev.azure.com/adamrpconnelly/Thrift.Net/_build/latest?definitionId=3&branchName=main).
The build is also configured to fail if the coverage drops below a certain
threshold. This isn't intended to force us to artificially add tests that don't
make sense, but is there as a safety net so that we need to make a conscious
decision about not testing certain components.

If a PR is added that causes the coverage to drop below this threshold, we
should have a conversation about whether we just need to add tests to increase
the coverage, or whether it makes sense to adjust the coverage settings.

If we decide to adjust the settings, we have the following options:

- Lowering the coverage threshold.
- Adjusting the coverage settings to exclude certain parts of the code from
  analysis.

In general we'll want to avoid adjusting the threshold, and instead should focus
on excluding any code that should not or cannot be tested. This is configured
via the
[Thrift.Net.Tests.runsettings](/src/Thrift.Net.Tests/Thrift.Net.Tests.runsettings)
file.

To provide a concrete example of this, the `Thrift.Net.Antlr` assembly is
excluded from coverage via the following pattern:

```text
[Thrift.Net.Antlr]*
```

The reason that this assembly is excluded is because it only contains generated
code, and we don't use all of the functionality that Antlr generates. This ends
up skewing our code coverage stats, and attempting to add tests for the unused
functionality would be pointless, and would also result in us testing 3rd party
library code.

### Testing Language Features

To test that the compiler parses a piece of Thrift code correctly, you can
create a new instance of the compiler, parse an input string, and then verify
that the correct results are produced:

```csharp
// Arrange
var compiler = new ThriftCompiler();
var input = "struct User {}";

// Act
var result = compiler.Compile(input.ToStream());

// Assert
Assert.Collection(
    result.Document.Structs,
    item => Assert.Equal("User", item.Name));
```

### Testing Compiler Messages

When testing that the compiler produces the correct messages, we want to check a
few things:

- The message Id and message text are correct.
- The message level is correct (for example _Error_ or _Warning_).
- The line and character positions are correct.

The `ThriftCompilerTests` class has a number of helper methods like
`AssertCompilerReturnsErrorMessage()` to help with this. Here's an example
showing how to check the compiler returns an error if an enum member is missing
its name:

```csharp
[Fact]
public void Compile_EnumMemberNameMissing_ReportsError()
{
    this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
  User = 0,
  $= 1$
}",
CompilerMessageId.EnumMemberMustHaveAName);
}
```

As you can see, part of the input text is surrounded by `$` signs (`$= 1$`).
This allows the test to automatically detect the line and column range where the
error message should be reported.

### XUnit Theories

There are some situations where we want to perform the same test against a range
of input values. To do this we can use XUnit Theories. An example of this is
where we want to ensure that the `BaseTypeBinder` can bind all the Thrift base
types:

```csharp
[Theory]
[InlineData("bool")]
[InlineData("byte")]
[InlineData("i8")]
[InlineData("i16")]
[InlineData("i32")]
[InlineData("i64")]
[InlineData("double")]
[InlineData("string")]
[InlineData("binary")]
[InlineData("slist")]
public void CanBindAllBaseTypes(string baseType)
{
    // Arrange
    var binder = new BaseTypeBinder();
    var fieldTypeNode = ParserInput
        .FromString(baseType)
        .ParseInput(parser => parser.baseType());

    // Act
    var parent = Substitute.For<IField>();
    var type = binder.Bind<BaseType>(fieldTypeNode, parent);

    // Assert
    Assert.Equal(baseType, type.Name);
}
```

## End to End Testing

Although unit tests cover a lot of our needs, there are certain scenarios that
we either can't test via unit tests, or where a unit testing approach is
inefficient or difficult. This includes:

- Testing parts of the code generation process that rely on compiling and
  running the generated code.
- Integration testing to ensure that Thrift.Net clients and servers can
  communicate correctly.
- Integration testing to ensure that Thrift.Net can communicate with other
  Thrift implementations.

We will update this section with more detail once the Thrift compiler is
completed, and we have the basic functionality in place to support creating
integration tests.
