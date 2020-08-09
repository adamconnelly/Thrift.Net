namespace csharp Thrift.Net.Samples

enum UserType {
    User,
    Administrator,
    Reseller
}

enum Permissions {
    CanRead = 5,
    CanWrite = 4,
    CanExecute = 8
}

enum Status {
    Success = 2,
    Failure
}
