namespace netstd Thrift.Net.Examples.Exceptions

enum ObjectType {
    User = 0
    Order = 1
}

exception NotFoundException {
    1: ObjectType Type
    2: string Id
    3: string Message
}
