using System;
using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;

namespace DSoft.Portable.WebClient.Rest.Encryption;

/// <summary>
/// Concrete REST secure response: a standard <see cref="ResponseBase"/> result that also carries an
/// encrypted <see cref="SecurePayload"/> back to the client.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Rest.ResponseBase" />
/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureResponse{T}" />
public class SecureResponse : ResponseBase, ISecureResponse<SecurePayload>
{
    /// <summary>
    /// The encrypted payload being returned.
    /// </summary>
    public SecurePayload Payload { get; set; }

    /// <summary>
    /// Creates a successful response with an empty, freshly time-stamped payload.
    /// </summary>
    public SecureResponse()
    {
        Success = true;

        Payload = new SecurePayload();
    }

    /// <summary>
    /// Stores an already-encrypted string as the response payload.
    /// </summary>
    /// <param name="data">The pre-encrypted payload string.</param>
    /// <exception cref="System.Exception">Thrown when the response has no payload to populate.</exception>
    public void SetPayload(string data)
    {
        if (Payload == null)
            throw new Exception("Payload is not set");

        Payload.Data = data;
    }

    /// <summary>
    /// Encrypts an object and stores it as the response payload.
    /// </summary>
    /// <param name="data">The object to encrypt and return.</param>
    /// <param name="passKey">The pass phrase to encrypt with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public void SetPayload(object data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) => SetPayload(PayloadManager.EncryptPayload(data, passKey, initVector, keySize));

    /// <summary>
    /// Decrypts the response payload and deserializes it into <typeparamref name="TData"/>.
    /// </summary>
    /// <typeparam name="TData">The type the encrypted payload represents.</typeparam>
    /// <param name="passKey">The pass phrase the payload was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The decrypted, deserialized payload.</returns>
    /// <exception cref="System.Exception">Thrown when the response has no payload to extract.</exception>
    public TData Extract<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        if (Payload == null)
            throw new Exception("No data");

        return Payload.Extract<TData>(passKey, initVector, keySize);
    }


}
