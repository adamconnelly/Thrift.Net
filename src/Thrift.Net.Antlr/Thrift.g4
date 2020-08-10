grammar Thrift;

document: header definitions;

header: (includeStatement | cppIncludeStatement | namespaceStatement)*;

includeStatement: 'include' LITERAL;
cppIncludeStatement: 'cppInclude' LITERAL;
namespaceStatement: NAMESPACE (namespaceScope=.*? | namespaceScope='*') ns=IDENTIFIER;

definitions: definition*;

definition: enumDefinition;

// TODO: Warning for empty enum
enumDefinition: ENUM IDENTIFIER?
    '{'
        enumMember*
    '}';

enumMember: (
        IDENTIFIER (EQUALS_OPERATOR enumValue=.*?)? | // Successful parse
        EQUALS_OPERATOR enumValue=.*? |               // `= 123` (missing name)
        IDENTIFIER enumValue=.*?                      // `User 123` (missing =)
    ) LIST_SEPARATOR?;

NAMESPACE: 'namespace';
ENUM: 'enum';
EQUALS_OPERATOR: '=';
LITERAL: ( '"' .*? '"' ) | ( '\'' .*? '\'' );
IDENTIFIER: ( [a-zA-Z] | '_' ) ( [a-zA-Z] | [0-9] | '.' | '_' )*;
INT_CONSTANT: ('+' | '-')? [0-9]+;
LIST_SEPARATOR: ',' | ';';

// TODO: Do we need this?
WS: [ \t\r\n]+ -> skip;
