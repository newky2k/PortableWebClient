

namespace DSoft.Portable.WebClient.Rest.Enums;

/// <summary>
/// The authentication scheme a REST client applies to its requests.
/// </summary>
public enum RequestAuthenticationType
{
    /// <summary>
    /// No authentication; requests are sent without credentials.
    /// </summary>
    Anonymous,
    /// <summary>
    /// Cookie-based authentication, where a session cookie is attached to each request.
    /// </summary>
    Cookie,
    /// <summary>
    /// JWT bearer-token authentication, where a token is sent in the Authorization header.
    /// </summary>
    Token,
}
