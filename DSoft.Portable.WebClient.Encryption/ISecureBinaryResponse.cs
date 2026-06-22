namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// A secure response that returns an encrypted binary file along with its name and MIME type,
/// for example when a service streams a download back to the client.
/// </summary>
/// <typeparam name="T">The concrete secure payload type carried by the response.</typeparam>
/// <typeparam name="T2">The wrapper type that holds the encrypted binary data.</typeparam>
/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureResponse{T}" />
public interface ISecureBinaryResponse<T, T2> : ISecureResponse<T> where T : ISecurePayload
{
    /// <summary>
    /// The file name to present to the caller for the returned content.
    /// </summary>
    string FileName { get; set; }

    /// <summary>
    /// The MIME type describing the returned content.
    /// </summary>
    string MimeType { get; set; }

    /// <summary>
    /// The encrypted binary content being returned.
    /// </summary>
    T2 Data { get; set; }
}
