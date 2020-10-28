struct Address {
    1: string Line1
    2: string Line2
    3: string PostCode
    4: string Country
}

enum PermissionType {
    Create = 0
    Update = 1
    Delete = 2
}

struct User {
    1: i32 Id,
    2: list<string> Emails,
    3: list<Address> Addresses,
    4: list<string> SomeList
    5: list<list<Address>> NestedList
    6: set<string> PhoneNumbers,
    7: set<PermissionType> Permissions
    8: set<list<set<bool>>> NestedSet
    9: map<string, bool> Map
    10: map<set<string>, map<string, PermissionType>> NestedMap
}
