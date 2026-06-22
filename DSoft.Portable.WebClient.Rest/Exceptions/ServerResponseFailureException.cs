using System;
using System.Net;

namespace DSoft.Portable.WebClient.Rest.Exceptions;

/// <summary>
/// Thrown when the server responds with a non-success HTTP status code, capturing the status
/// and error detail returned.
/// </summary>
/// <seealso cref="Exception" />
public class ServerResponseFailureException : Exception
{
    private HttpStatusCode _httpStatusCode;
    private string _errorMessage;
    /// <summary>
    /// A failure message combining the returned HTTP status code and the server's error detail.
    /// </summary>
    public override string Message
    {
        get
        {
            return $"Response failure - Status: {_httpStatusCode} - Message - {_errorMessage}";
        }
    }

    /// <summary>
    /// Creates the exception from the failed HTTP response.
    /// </summary>
    /// <param name="httpStatusCode">The non-success status code returned by the server.</param>
    /// <param name="errorMessage">The error detail returned with the response.</param>
    /// <param name="exception">The underlying exception, if any.</param>
    public ServerResponseFailureException(HttpStatusCode httpStatusCode, string errorMessage, Exception exception) : base(null, exception)
    {
        _httpStatusCode = httpStatusCode;
        _errorMessage = errorMessage;
    }
}
