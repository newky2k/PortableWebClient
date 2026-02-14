
namespace DSoft.Portable.WebClient.Core.Enum;

/// <summary>
/// Authentication Type for request
/// </summary>
public enum RequestAuthenticationType
{
    /// <summary>
    /// Specifies that no authentication is required for the request.
    /// </summary>
    None,
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
