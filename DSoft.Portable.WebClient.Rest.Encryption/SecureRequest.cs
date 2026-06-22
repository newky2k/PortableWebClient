using System;
using DSoft.Portable.WebClient.Encryption;

namespace DSoft.Portable.WebClient.Rest.Encryption;

/// <summary>
/// Concrete REST secure request: a versioned request body wrapping an encrypted <see cref="SecurePayload"/>,
/// with identifiers for correlation and token-based routing.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Rest.RequestBase" />
/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureRequest{T}" />
public class SecureRequest : RequestBase, ISecureRequest<SecurePayload>
{
    /// <summary>
    /// A correlation identifier for the request.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The identifier of the authentication token the request is made under.
    /// </summary>
    public string TokenId { get; set; }

    /// <summary>
    /// The encrypted payload being sent.
    /// </summary>
    public SecurePayload Payload { get; set; }

    /// <summary>
    /// An optional caller-defined context string passed alongside the request.
    /// </summary>
    public string Context { get; set; }

    /// <summary>
    /// Creates a request with an empty, freshly time-stamped payload.
    /// </summary>
    public SecureRequest()
    {
        Payload = new SecurePayload();
    }

    /// <summary>
    /// Creates a request whose payload is an encrypted empty placeholder, for calls that send no data but still require encryption.
    /// </summary>
    /// <param name="passKey">The pass phrase used to encrypt the payload.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public SecureRequest(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        Payload = new SecurePayload(passKey, initVector, keySize);
    }

    /// <summary>
    /// Creates a request whose payload is <paramref name="data"/> serialized and encrypted.
    /// </summary>
    /// <param name="passKey">The pass phrase used to encrypt the payload.</param>
    /// <param name="data">The object to encrypt into the payload.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public SecureRequest(string passKey, object data, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        Payload = new SecurePayload(data, passKey, initVector, keySize);
    }

    /// <summary>
    /// Creates an empty-payload request stamped with an explicit timestamp instead of the current time.
    /// </summary>
    /// <param name="passKey">The pass phrase used to encrypt the payload.</param>
    /// <param name="timestamp">The timestamp to apply to the payload for freshness checks.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public SecureRequest(string passKey, DateTime timestamp, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(passKey, initVector, keySize)
    {
        Payload.Timestamp = timestamp;
    }

    /// <summary>
    /// Creates an empty-payload request that also records the calling client's version.
    /// </summary>
    /// <param name="passKey">The pass phrase used to encrypt the payload.</param>
    /// <param name="clientVersionNumber">The version of the client making the request.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public SecureRequest(string passKey, string clientVersionNumber, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(passKey, initVector, keySize)
    {
        ClientVersionNo = clientVersionNumber;
    }

    /// <summary>
    /// Creates an empty-payload request that records both the client version and an explicit timestamp.
    /// </summary>
    /// <param name="passKey">The pass phrase used to encrypt the payload.</param>
    /// <param name="clientVersionNumber">The version of the client making the request.</param>
    /// <param name="timestamp">The timestamp to apply to the payload for freshness checks.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public SecureRequest(string passKey, string clientVersionNumber, DateTime timestamp, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(passKey, clientVersionNumber, initVector, keySize)
    {
        Payload.Timestamp = timestamp;
    }

    /// <summary>
    /// Ensures the request carries a payload that is present and still within the allowed age.
    /// </summary>
    /// <param name="timeout">The maximum age the payload may have.</param>
    /// <exception cref="System.Exception">Thrown when there is no payload, or the payload is unstamped or older than <paramref name="timeout"/>.</exception>
    public void Validate(TimeSpan timeout)
    {
        if (Payload == null)
            throw new Exception("No payload in request");

        if (Payload.Timestamp == default || !Payload.Validate(timeout))
            throw new Exception("Payload has timed out");
    }

    /// <summary>
    /// Decrypts the request's payload and deserializes it into <typeparamref name="TData"/>.
    /// </summary>
    /// <typeparam name="TData">The type the encrypted payload represents.</typeparam>
    /// <param name="passKey">The pass phrase the payload was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The decrypted, deserialized payload.</returns>
    /// <exception cref="System.Exception">Thrown when the request has no payload to extract.</exception>
    public TData ExtractPayload<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        if (Payload == null)
            throw new Exception("No data");

        return Payload.Extract<TData>(passKey, initVector, keySize);
    }

}
