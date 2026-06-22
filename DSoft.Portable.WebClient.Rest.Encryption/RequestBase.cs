namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Base for request bodies that carry the calling client's version so the server can enforce compatibility.
/// </summary>
public abstract class RequestBase
{
    /// <summary>
    /// The version of the client making the request, used for server-side version checks.
    /// </summary>
    public string ClientVersionNo { get; set; }
}
