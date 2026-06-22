using DSoft.Portable.WebClient.Encryption;

namespace DSoft.Portable.WebClient.Rest.Encryption;

/// <summary>
/// <see cref="RestApiClientOptions"/> extended with the encryption settings a secure REST client needs.
/// </summary>
public class SecureRestApiClientOptions : RestApiClientOptions
{
    /// <summary>
    /// The key size the cipher should use when encrypting and decrypting payloads.
    /// </summary>
    public KeySize KeySize { get; set; }
}
