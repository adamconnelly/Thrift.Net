namespace netstd Thrift.Net.Examples.Exceptions

exception {
    1: string First
    2: optional string Second
    3: required string Third
    i32 Fifth
}

struct Request {}
exception Request {}
