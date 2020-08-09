grammar Thrift;

document: header definitions;

header: (includeStatement | cppIncludeStatement | namespaceStatement)*;

includeStatement: 'include' LITERAL;
cppIncludeStatement: 'cppInclude' LITERAL;
namespaceStatement: 'namespace' namespaceScope IDENTIFIER;

// Although we'll parse all the allowed namespaces, we'll only allow *, csharp
// and netcore, and we'll treat csharp and netcore as equivalent
// TODO: potentially switch to '*' | IDENTIFIER and handle in code
namespaceScope: '*' |
    'c_glib' |
    'cpp' |
    'csharp' |
    'delphi' |
    'go' |
    'java' |
    'js' |
    'lua' |
    'netcore' |
    'perl' |
    'php' |
    'py' |
    'py.twisted' |
    'rb' |
    'st' |
    'xsd';

definitions: definition*;

definition: enumDefinition;

// TODO: Warning for empty enum
enumDefinition: ENUM IDENTIFIER?
    '{'
        enumMember*
    '}';

enumMember: (
        IDENTIFIER (EQUALS_OPERATOR enumValue=.*?)? |
        EQUALS_OPERATOR enumValue=.*?
    ) LIST_SEPARATOR?;

ENUM: 'enum';
EQUALS_OPERATOR: '=';
LITERAL: ( '"' .*? '"' ) | ( '\'' .*? '\'' );
IDENTIFIER: ( [a-zA-Z] | '_' ) ( [a-zA-Z] | [0-9] | '.' | '_' )*;
INT_CONSTANT: ('+' | '-')? [0-9]+;
LIST_SEPARATOR: ',' | ';';

// TODO: Do we need this?
WS: [ \t\r\n]+ -> skip;
