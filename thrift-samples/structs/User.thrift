namespace netstd Thrift.Net.Examples

enum UserType {
    User = 0
    Administrator = 2
}

struct Address {
    1: string Line1
    2: string Line2
    3: string Town
    4: string PostCode
    5: string Country
}

enum PermissionType {
    Read = 0
    Write = 1
    Execute = 2
}

enum PhoneType {
    Home,
    Mobile
}

struct Permission {
    1: PermissionType Type
    2: string Name
}

struct User {
    1: i32 Id
    2: required UserType Type
    3: list<Address> Addresses
    4: set<Permission> Permissions
    5: UserType SecondaryType
    6: set<string> Emails
    7: map<PhoneType, string> PhoneNumbers
}
