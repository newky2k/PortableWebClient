using System;

namespace DSoft.Portable.WebClient.Rest.Exceptions;

/// <summary>
/// Thrown when the server returns a response but it indicates a failure or its body cannot be
/// turned into the expected data.
/// </summary>
/// <seealso cref="Exception" />
public class DataResponseFailureException : Exception
{
    /// <summary>
    /// Creates the exception with the failure detail reported by the server.
    /// </summary>
    /// <param name="errorMessage">A description of the data failure.</param>
    public DataResponseFailureException(string errorMessage) : base(errorMessage)
    {

    }
}
