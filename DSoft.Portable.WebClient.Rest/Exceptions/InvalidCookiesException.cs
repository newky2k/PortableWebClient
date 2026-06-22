using System;

namespace DSoft.Portable.WebClient.Rest.Exceptions;

/// <summary>
/// Thrown during cookie-authentication preflight when the required session cookies are missing
/// or no longer valid.
/// </summary>
/// <seealso cref="System.Exception" />
public class InvalidCookiesException : Exception
{
    /// <summary>
    /// Creates the exception with a default "invalid or missing cookies" message.
    /// </summary>
    public InvalidCookiesException() : base("Cookies are invalid or missing")
    {

    }
}
