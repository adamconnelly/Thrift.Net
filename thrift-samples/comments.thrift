# This file contains examples of the different types of comments supported
# by Thrift.
namespace * Thrift.Net.Examples

// Describes the different types of user supported by the system
enum UserType {
    /*
     * An administrator - can perform any action.
     */
    Administrator

    // A standard user
    User
}

/// <summary>
/// Represents a user.
/// </summary>
struct User {
    /**
     * Gets or sets the user's Id.
     */
    0: i32 Id

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    1: string Username

    // Gets or sets the user's type.
    2: UserType Type
}
