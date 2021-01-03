namespace netstd Thrift.Net.Examples

const i32 = 100
const i32 MaxPageSize = 100
const i32 MinPageSize = 0x0A
const i8 InvalidType = 1000000
const i16 I32ToI16 = -32769
const i32 I64ToI32 = 2147483648
const i32 StringToI32 = "test"
//const i32 DoubleToInt = 10e2 // this causes an error even though 10e2 would produce an integer
const string DefaultAdminUsername = "admin"
//const bool IsEnabledByDefault = true
//const list<string> Permissions = [ "Pages", "Articles", "Users" ]
//const set<i32> MagicNumbers = [ 1, 5, 2, 10, 12 ]
//const map<string, string> Emails = { "work": "worker@work.com", "personal": "person@personal.com" }

/*
enum UserType {
    User = 3
    Administrator = 7
}

struct User {
    1: i32 Id
    2: UserType Type
}

const UserType DefaultUserType = UserType.Administrator
const User DefaultUser = { "Id": 123, "Type": UserType.User }
*/
