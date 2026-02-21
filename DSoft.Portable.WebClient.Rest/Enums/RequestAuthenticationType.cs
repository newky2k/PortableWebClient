

namespace DSoft.Portable.WebClient.Rest.Enums;

/// <summary>
/// Authentication Type for request
/// </summary>
public enum RequestAuthenticationType
{
    /// <summary>
    /// Represents an anonymous type that encapsulates a set of read-only properties.
    /// </summary>
    /// <remarks>Anonymous types are typically used to create simple objects for data transfer without
    /// explicitly defining a class. They are immutable and can only be created using object initializers.</remarks>
    Anonymous,
    /// <summary>
    /// Represents an HTTP cookie used to store data on the client and send it to the server with subsequent requests.
    /// </summary>
    /// <remarks>Cookies are commonly used for session management, personalization, and tracking. They can
    /// have attributes such as expiration, path, and domain that determine their behavior and scope.</remarks>
    Cookie,
    /// <summary>
    /// Use JWT Token based authentication
    /// </summary>
    Token,
}
