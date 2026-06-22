using System;

namespace DSoft.Portable.WebClient.Rest.Exceptions;

/// <summary>
/// Thrown when the client cannot reach the server at all (no HTTP response was received),
/// for example due to a connection failure or timeout.
/// </summary>
/// <seealso cref="Exception" />
public class NoServerResponseException : Exception
{
    private string _errorMessage;
    /// <summary>
    /// A connection-failure message that wraps the underlying error detail.
    /// </summary>
    public override string Message
    {
        get
        {
            return $"Unable to connect - Message - {_errorMessage}";
        }
    }

    /// <summary>
    /// Creates the exception from the underlying transport error.
    /// </summary>
    /// <param name="errorMessage">A description of the connection failure.</param>
    /// <param name="exception">The underlying exception that caused the failure.</param>
    public NoServerResponseException(string errorMessage, Exception exception) : base(null, exception)
    {
        _errorMessage = errorMessage;
    }
}
