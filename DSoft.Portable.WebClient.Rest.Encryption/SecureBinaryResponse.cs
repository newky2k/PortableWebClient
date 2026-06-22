using DSoft.Portable.WebClient.Encryption;

namespace DSoft.Portable.WebClient.Rest.Encryption;

/// <summary>
/// A <see cref="SecureResponse"/> that returns an encrypted binary file together with its name and MIME
/// type, used when a service streams a download back to the client.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Rest.Encryption.SecureResponse" />
/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureBinaryResponse{T, T2}" />
public class SecureBinaryResponse : SecureResponse, ISecureBinaryResponse<SecurePayload, byte[]>
{
    /// <summary>
    /// The file name to present to the caller for the returned content.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// The MIME type describing the returned content.
    /// </summary>
    public string MimeType { get; set; }

    /// <summary>
    /// The encrypted binary content being returned.
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    /// Creates an empty binary response; populate the file metadata and content before returning it.
    /// </summary>
    public SecureBinaryResponse()
    {

    }
}
