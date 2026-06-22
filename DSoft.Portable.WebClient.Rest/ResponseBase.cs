namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Base type for all API responses, carrying the success flag and optional message that every
/// response shares. Concrete responses add their own payload properties.
/// </summary>
public abstract class ResponseBase
{
    /// <summary>
    /// Whether the operation succeeded. Defaults to <c>true</c> and is set to <c>false</c> when the server reports a failure.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// An optional human-readable message, typically describing the failure when <see cref="Success"/> is <c>false</c>.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Creates a response that is successful by default.
    /// </summary>
    public ResponseBase()
    {
        Success = true;
    }
}
