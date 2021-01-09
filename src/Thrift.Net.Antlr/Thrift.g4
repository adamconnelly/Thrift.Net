grammar Thrift;

document: header definitions EOF;

header: (includeStatement | cppIncludeStatement | namespaceStatement)*;

includeStatement: 'include' LITERAL;
cppIncludeStatement: 'cpp_include' LITERAL;

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
    ) listSeparator?; // LIST_SEPARATOR is not part of the Thrift spec,
                                 // but allowing it lets us handle the situation
                                 // where someone has added a separator by accident

definitions: (
    enumDefinition |
    structDefinition |
    unionDefinition |
    exceptionDefinition |
    constDefinition)*;

enumDefinition: ENUM name=IDENTIFIER?
    '{'
        enumMember*
    '}';

enumMember: (
        IDENTIFIER (EQUALS_OPERATOR enumValue=.*?)? | // Successful parse
        EQUALS_OPERATOR enumValue=.*? |               // `= 123` (missing name)
        IDENTIFIER enumValue=.*?                      // `User 123` (missing =)
    ) listSeparator?;

structDefinition: STRUCT name=IDENTIFIER?
    '{'
        field*
    '}';

unionDefinition: UNION name=IDENTIFIER?
    '{'
        field*
    '}';

exceptionDefinition: EXCEPTION name=IDENTIFIER?
    '{'
        field*
    '}';

field: (fieldType name=IDENTIFIER |
    fieldRequiredness fieldType name=IDENTIFIER |
    fieldId=(INT_CONSTANT | IDENTIFIER | LITERAL)? ':' fieldRequiredness? fieldType name=IDENTIFIER) listSeparator?;

fieldRequiredness: REQUIRED | OPTIONAL;

fieldType: baseType | userType | collectionType;
baseType: typeName=('bool' | 'byte' | 'i8' | 'i16' | 'i32' | 'i64' | 'double' | 'string' | 'binary' | 'slist');
userType: IDENTIFIER;
collectionType: listType | setType | mapType;
listType: LIST LT_OPERATOR? fieldType? GT_OPERATOR?;
setType: SET LT_OPERATOR? fieldType? GT_OPERATOR?;
mapType: MAP LT_OPERATOR? keyType=fieldType? COMMA? valueType=fieldType? GT_OPERATOR?;

constDefinition:
    CONST fieldType name=IDENTIFIER EQUALS_OPERATOR? | // `const i32 MaxPageSize =` (missing initialization expression)
    CONST fieldType EQUALS_OPERATOR constExpression | // `const i32 = 100` (missing name)
    CONST fieldType name=IDENTIFIER constExpression | // `const i32 MaxPageSize 100` (missing '=' operator)
    CONST fieldType name=IDENTIFIER EQUALS_OPERATOR constExpression // Successful parse
    ;
constExpression: value=INT_CONSTANT |
    value=HEX_CONSTANT |
    value=DOUBLE_CONSTANT |
    value=BOOL_CONSTANT |
    value=LITERAL;

listSeparator: COMMA | SEMICOLON;

NAMESPACE: 'namespace';
ENUM: 'enum';
STRUCT: 'struct';
UNION: 'union';
EXCEPTION: 'exception';
REQUIRED: 'required';
OPTIONAL: 'optional';
LIST: 'list';
SET: 'set';
MAP: 'map';
CONST: 'const';
LT_OPERATOR: '<';
GT_OPERATOR: '>';
COMMA: ',';
SEMICOLON: ';';
KNOWN_NAMESPACE_SCOPES: '*' | 'c_glib' | 'cpp' | 'csharp' | 'delphi' | 'go' |
    'java' | 'js' | 'lua' | 'netcore' | 'netstd' | 'perl' | 'php' | 'py' |
    'py.twisted' | 'rb' | 'st' | 'xsd';
EQUALS_OPERATOR: '=';

// Examples of valid doubles (based on the Thrift IDL) include:
// 100.5
// -100.5
// +100.5
// 100.5e10
// 100e2 (for some reason only doubles support exponent syntax, and an integer value using this syntax is classed as a double)
DOUBLE_CONSTANT: ('+' | '-')? [0-9]* (
        ('.' [0-9]+) ([eE] INT_CONSTANT)? |
        ([eE] INT_CONSTANT)
    );

INT_CONSTANT: ('+' | '-')? [0-9]+;

// NOTE: HEX_CONSTANT deliberately allows invalid hex (i.e. letters > F) to allow
// for graceful error handling
HEX_CONSTANT: '0' [xX] [0-9a-zA-Z]+;

// NOTE: Thrift also allows integer boolean expressions, but we parse that as an
// int and handle it in the compiler
BOOL_CONSTANT: 'true' | 'false';

LITERAL: ( '"' .*? '"' ) | ( '\'' .*? '\'' );
IDENTIFIER: ( [a-zA-Z] | '_' ) ( [a-zA-Z] | [0-9] | '.' | '_' )*;

WS: [ \t\r\n]+ -> skip;
COMMENT: (
    // C++-style or shell-style, single-line comments
    ( '//' | '#' ) ~[\r\n]* |

    // C-style, multi-line comment
    '/*' .*? '*/') -> skip;
