grammar Thrift;

document: header definitions EOF;

header: (includeStatement | cppIncludeStatement | namespaceStatement)*;

includeStatement: 'include' LITERAL;
cppIncludeStatement: 'cppInclude' LITERAL;

namespaceStatement: (
        // Successful parse
        // `namespace csharp Thrift.Net.Examples`
        NAMESPACE namespaceScope=KNOWN_NAMESPACE_SCOPES ns=IDENTIFIER |

        // Missing namespace
        // `namespace csharp`
        NAMESPACE namespaceScope=KNOWN_NAMESPACE_SCOPES |

        // Various other error scenarios
        // `namespace`
        // `namespace fortran Thrift.Net.Examples`
        // `namespace Thrift.Net.Examples`
        // `namespace fortran`
        NAMESPACE namespaceScope=.*? ns=IDENTIFIER?
    ) separator=LIST_SEPARATOR?; // LIST_SEPARATOR is not part of the Thrift spec,
                                 // but allowing it lets us handle the situation
                                 // where someone has added a separator by accident

definitions: (enumDefinition | structDefinition)*;

enumDefinition: ENUM name=IDENTIFIER?
    '{'
        enumMember*
    '}';

enumMember: (
        IDENTIFIER (EQUALS_OPERATOR enumValue=.*?)? | // Successful parse
        EQUALS_OPERATOR enumValue=.*? |               // `= 123` (missing name)
        IDENTIFIER enumValue=.*?                      // `User 123` (missing =)
    ) LIST_SEPARATOR?;

structDefinition: STRUCT name=IDENTIFIER?
    '{'
        field*
    '}';

field: (fieldType name=IDENTIFIER |
    fieldRequiredness fieldType name=IDENTIFIER |
    fieldId=(INT_CONSTANT | IDENTIFIER | LITERAL)? ':' fieldRequiredness? fieldType name=IDENTIFIER) LIST_SEPARATOR?;

fieldRequiredness: REQUIRED | OPTIONAL;
fieldType: IDENTIFIER;

NAMESPACE: 'namespace';
ENUM: 'enum';
STRUCT: 'struct';
REQUIRED: 'required';
OPTIONAL: 'optional';
KNOWN_NAMESPACE_SCOPES: '*' | 'c_glib' | 'cpp' | 'csharp' | 'delphi' | 'go' |
    'java' | 'js' | 'lua' | 'netcore' | 'netstd' | 'perl' | 'php' | 'py' |
    'py.twisted' | 'rb' | 'st' | 'xsd';
EQUALS_OPERATOR: '=';
LITERAL: ( '"' .*? '"' ) | ( '\'' .*? '\'' );
IDENTIFIER: ( [a-zA-Z] | '_' ) ( [a-zA-Z] | [0-9] | '.' | '_' )*;
INT_CONSTANT: ('+' | '-')? [0-9]+;
LIST_SEPARATOR: ',' | ';';

// TODO: Do we need this?
WS: [ \t\r\n]+ -> skip;
