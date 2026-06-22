using System;

namespace DSoft.Portable.WebClient.Rest.Exceptions;

/// <summary>
/// Thrown when the server rejects a request as unauthorized (HTTP 401/403), indicating the
/// caller lacks access to the requested resource.
/// </summary>
/// <seealso cref="System.Exception" />
public class UnauthorisedException : Exception
{
    /// <summary>
    /// Creates the exception with a default "no access" message.
    /// </summary>
    public UnauthorisedException() : base("You do not have access to this rsesource")
    {

    }
}
