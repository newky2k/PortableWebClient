namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// A response envelope that carries an encrypted <see cref="ISecurePayload"/> back to the client,
/// with helpers to populate and decrypt the payload.
/// </summary>
/// <typeparam name="T">The concrete secure payload type carried by the response.</typeparam>
public interface ISecureResponse<T> where T : ISecurePayload
{
    /// <summary>
    /// The encrypted payload being returned.
    /// </summary>
    T Payload { get; set; }

    /// <summary>
    /// Encrypts an object and stores it as the response payload.
    /// </summary>
    /// <param name="data">The object to encrypt and return.</param>
    /// <param name="passKey">The pass phrase to encrypt with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    void SetPayload(object data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);

    /// <summary>
    /// Stores an already-encrypted string as the response payload.
    /// </summary>
    /// <param name="data">The pre-encrypted payload string.</param>
    void SetPayload(string data);

    /// <summary>
    /// Decrypts the response payload and deserializes it into <typeparamref name="TData"/>.
    /// </summary>
    /// <typeparam name="TData">The type the encrypted payload represents.</typeparam>
    /// <param name="passKey">The pass phrase the payload was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The decrypted, deserialized payload.</returns>
    TData Extract<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
}
