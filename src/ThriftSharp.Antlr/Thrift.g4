grammar Thrift;

document: header* definition*;

header: include | cppInclude | namespace;

include: 'include' LITERAL;
cppInclude: 'cppInclude' LITERAL;
namespace: 'namespace' namespaceScope IDENTIFIER;

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

definition: 'doc';

LITERAL: ('"' [^"]* '"') | ('\'' [^']* '\'');
IDENTIFIER: ( LETTER | '_' ) ( LETTER | DIGIT | '.' | '_' )*;
LETTER: [a-z][A-Z];
DIGIT: [0-9];
