# Binding

Binding is the process of mapping between tokens in the syntax tree and Symbols.
Symbols provide a richer view of the input code than we can get from the parse
tree alone, and allow us to do things like detect duplicate identifiers, and
match type names with their definitions.

Binders use a tree structure to represent the scope rules in the Thrift IDL, so
that we can detect duplicate identifiers, and also find symbols that were
defined in a higher scope.

```text
Root
  --> ThriftDocumentBinder
    --> ImportBinder
    --> NamespaceBinder
    --> EnumBinder
      --> EnumMemberBinder
    --> StructBinder
      --> FieldBinder
    --> ExceptionBinder
      --> FieldBinder
    --> ServiceBinder
      --> ParameterListBinder
```

When we perform further analysis of the code to report errors and warnings, we
can use the Binder for the current scope to get semantic information about the
code, for example the value of the current enum member, or whether it has been
declared multiple times.

## Symbols

Symbols represent the different types of Thrift object, like structs, enums and
fields. Each symbol is represented by a class implementing `ISymbol`.

## Binders

Each Symbol has an associated Binder, that knows how to create the Symbol based
on a node in the parse tree.

## Binder Provider

The `BinderProvider` is used to map between a node in the parse tree and the
Binder used to bind it to a Symbol. The BinderProvider uses a visitor to visit
nodes in the parse tree and associate them with the correct Binder.

## Type Resolution

Binders support resolving types using the `ResolveType()` method. This returns a
`FieldType` object containing information about the type. Because Binders are
built in a tree structure to represent the scope, type resolution can walk up
this tree until it finds a binder that can resolve the specified type.

For example, if we have the following Thrift definition:

```thrift
enum UserType {
  User
  Administrator
}

struct User {
  1: $UserType$ Type
}
```

If we try to resolve the `UserType` token surrounded by the `$` signs,
resolution will take the following path:

```text
FieldTypeBinder
  --> FieldBinder
    --> StructBinder
      --> DocumentBinder
```
