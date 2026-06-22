namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// Abstraction over a symmetric cipher used to protect payloads exchanged with a service.
/// Implementations encrypt and decrypt both text and raw bytes using a caller-supplied pass phrase.
/// </summary>
public interface IEncryptionProvider
{
    /// <summary>
    /// Encrypts a UTF-8 string and returns the cipher text as a Base64-encoded string.
    /// </summary>
    /// <param name="plainText">The clear text to protect.</param>
    /// <param name="passPhrase">The secret used to derive the encryption key.</param>
    /// <returns>The Base64-encoded cipher text.</returns>
    string EncryptString(string plainText, string passPhrase);

    /// <summary>
    /// Encrypts a raw byte buffer.
    /// </summary>
    /// <param name="data">The clear data to protect.</param>
    /// <param name="passPhrase">The secret used to derive the encryption key.</param>
    /// <returns>The encrypted bytes.</returns>
    byte[] EncryptBytes(byte[] data, string passPhrase);

    /// <summary>
    /// Decrypts Base64-encoded cipher text produced by <see cref="EncryptString"/> back to clear text.
    /// </summary>
    /// <param name="cipherText">The Base64-encoded cipher text.</param>
    /// <param name="passPhrase">The secret used to derive the decryption key; must match the one used to encrypt.</param>
    /// <returns>The recovered clear text.</returns>
    string DecryptString(string cipherText, string passPhrase);

    /// <summary>
    /// Decrypts a byte buffer produced by <see cref="EncryptBytes"/> back to its original form.
    /// </summary>
    /// <param name="data">The encrypted bytes.</param>
    /// <param name="passPhrase">The secret used to derive the decryption key; must match the one used to encrypt.</param>
    /// <returns>The recovered clear bytes.</returns>
    byte[] DecryptBytes(byte[] data, string passPhrase);
}
