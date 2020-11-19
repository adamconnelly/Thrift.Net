namespace netstd Thrift.Net.Examples.Unions

union {
    1: string First
    2: optional string Second
    3: required string Third
    i32 Fifth
}

struct Request {}
union Request {}
