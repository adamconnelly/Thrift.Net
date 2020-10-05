namespace netstd Thrift.Net.Examples

enum UserType {
    User = 10
    Administrator = 10
}

struct Address {
    1: string Line1
    2: string Line2
    3: string Town
    4: string PostCode
    5: string Country
}

struct User {
    1: i32 Id
    2: required UserType Type
    3: Address Address
    4: PermissionType PermissionType
    5: UserType SecondaryType
    // 5: Unknown Unknown
}

enum PermissionType {
    Read
    Write
    Execute
}
