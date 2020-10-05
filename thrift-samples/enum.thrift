namespace * Thrift.Net.Samples

enum UserType {
    User = 0x01,
    Administrator,
    Reseller
}

enum Permissions {
    CanRead = 5,
    CanWrite = 4,
    CanExecute = 8
}

enum Status {
    Success = 0xA1,
    Failure = 0xa2
}

enum Empty {
}
