namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// A secure request that, in addition to its <see cref="ISecurePayload"/>, carries an encrypted
/// binary blob (for example a file or stream) alongside the structured payload.
/// </summary>
/// <typeparam name="T">The concrete secure payload type carried by the request.</typeparam>
/// <typeparam name="T2">The wrapper type that holds the encrypted binary data.</typeparam>
/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureRequest{T}" />
public interface ISecureBinaryRequest<T, T2> : ISecureRequest<T> where T : ISecurePayload
{
    /// <summary>
    /// The encrypted binary content attached to the request.
    /// </summary>
    T2 BinaryObject { get; set; }

    /// <summary>
    /// Encrypts a byte buffer and stores it as the request's binary content.
    /// </summary>
    /// <param name="data">The raw bytes to encrypt and attach.</param>
    /// <param name="passKey">The pass phrase to encrypt with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    void SetBinaryObject(byte[] data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);

    /// <summary>
    /// Decrypts and returns the attached binary content.
    /// </summary>
    /// <param name="passKey">The pass phrase the content was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The decrypted bytes.</returns>
    byte[] GetBinaryObject(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
}
