namespace netstd Thrift.Net.Examples.Unions

struct Struct {
    1: string First
    2: string Second
    3: optional i32 OptionalField
}

union Union {
    1: string First
    2: string Second
    3: optional i32 OptionalField
}
