# Binding

Binding is the process of mapping between tokens in the syntax tree and Symbols.
Symbols provide a richer view of the input code than we can get from the parse
tree alone, and allow us to do things like detect duplicate identifiers, and
match type names with their definitions.

## Symbols

Symbols represent the different types of Thrift object, like structs, enums and
fields. Each symbol is represented by a class implementing `ISymbol`.

Symbols are arranged in a tree structure, creating a hierarchical symbol table.
This allows us to walk up the tree when performing type resolution to find the
correct type being referenced. The symbol tree looks roughly like this:

```text
ThriftCompilation
  --> Document
    --> Enum
      --> EnumMember
    --> Struct
      --> Field
        --> FieldType
    --> Union
      --> Field
        --> FieldType
    --> Exception
      --> Field
        --> FieldType
    --> Service
      --> Function
        --> Field
          --> FieldType
```

## Binders

Each Symbol has an associated Binder, that knows how to create the Symbol based
on a node in the parse tree.

## Binder Provider

The `BinderProvider` is used to map between a node in the parse tree and the
Binder used to bind it to a Symbol. The BinderProvider uses a visitor to visit
nodes in the parse tree and associate them with the correct Binder.

## Type Resolution

Symbols support resolving types using the `ResolveType()` method. This returns a
`FieldType` object containing information about the type. Because Symbols are
built in a tree structure to represent the scope, type resolution can walk up
this tree until it finds a Symbol that can resolve the specified type.

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
FieldType
  --> Field
    --> Struct
      --> Document
```

Once resolution reaches the document, it can search all its members for a type
with the specified name, and resolve it.
