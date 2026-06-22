using System;

namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// Base class for <see cref="IEncryptionProvider"/> implementations. Holds the initialization vector
/// and key size and provides virtual no-op members so derived providers only override the transforms they support.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Encryption.IEncryptionProvider" />
public abstract class EncryptionProviderBase : IEncryptionProvider
{
    /// <summary>
    /// The initialization vector string supplied at construction, shared by all transforms.
    /// </summary>
    protected string _initVector;

    /// <summary>
    /// The key size used to derive the encryption key; defaults to 256-bit.
    /// </summary>
    protected KeySize _keysize = KeySize.TwoFiftySix;

    /// <summary>
    /// Initializes the base provider with the initialization vector and key size used for all transforms.
    /// </summary>
    /// <param name="initVector">The initialization vector string.</param>
    /// <param name="keySize">The key size to derive.</param>
    public EncryptionProviderBase(string initVector, KeySize keySize)
    {
        _initVector = initVector;
        _keysize = keySize;
    }

    /// <summary>
    /// Decrypts a byte buffer. Override in a derived provider; the base throws if not implemented.
    /// </summary>
    /// <param name="data">The encrypted bytes.</param>
    /// <param name="passPhrase">The secret used to derive the key.</param>
    /// <returns>The recovered clear bytes.</returns>
    /// <exception cref="System.NotImplementedException">Thrown when the derived provider does not support byte decryption.</exception>
    public virtual byte[] DecryptBytes(byte[] data, string passPhrase)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Decrypts cipher text. Override in a derived provider; the base throws if not implemented.
    /// </summary>
    /// <param name="cipherText">The cipher text.</param>
    /// <param name="passPhrase">The secret used to derive the key.</param>
    /// <returns>The recovered clear text.</returns>
    /// <exception cref="System.NotImplementedException">Thrown when the derived provider does not support string decryption.</exception>
    public virtual string DecryptString(string cipherText, string passPhrase)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Encrypts a byte buffer. Override in a derived provider; the base throws if not implemented.
    /// </summary>
    /// <param name="data">The clear bytes to encrypt.</param>
    /// <param name="passPhrase">The secret used to derive the key.</param>
    /// <returns>The encrypted bytes.</returns>
    /// <exception cref="System.NotImplementedException">Thrown when the derived provider does not support byte encryption.</exception>
    public virtual byte[] EncryptBytes(byte[] data, string passPhrase)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Encrypts clear text. Override in a derived provider; the base throws if not implemented.
    /// </summary>
    /// <param name="plainText">The clear text to encrypt.</param>
    /// <param name="passPhrase">The secret used to derive the key.</param>
    /// <returns>The cipher text.</returns>
    /// <exception cref="System.NotImplementedException">Thrown when the derived provider does not support string encryption.</exception>
    public virtual string EncryptString(string plainText, string passPhrase)
    {
        throw new NotImplementedException();
    }

}
