using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption.Providers;


/// <summary>
/// Default <see cref="IEncryptionProvider"/> implementation backed by AES in CBC mode.
/// The key is derived from the caller's pass phrase and the initialization vector is taken
/// from the UTF-8 bytes of the configured init vector string.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Encryption.EncryptionProviderBase" />
internal class AesEncryptionProvider : EncryptionProviderBase
{

    /// <summary>
    /// The IV bytes used for every transform, derived from the UTF-8 encoding of the configured init vector.
    /// </summary>
    private byte[] InitVectorBytes => Encoding.UTF8.GetBytes(_initVector);


    /// <summary>
    /// Creates a fresh AES algorithm instance configured for CBC mode.
    /// </summary>
    private SymmetricAlgorithm EncryptionAlgorithm
    {
        get
        {
            var aesAlgorithm = Aes.Create();
            aesAlgorithm.Mode = CipherMode.CBC;

            return aesAlgorithm;
        }
    }


    /// <summary>
    /// Initializes a new provider with the initialization vector and key size used for all transforms.
    /// </summary>
    /// <param name="initVector">The initialization vector string; its UTF-8 bytes seed the cipher's IV.</param>
    /// <param name="keySize">The AES key size to derive.</param>
    public AesEncryptionProvider(string initVector, KeySize keySize) : base(initVector, keySize)
    {

    }

    #region Private Methods

    /// <summary>
    /// Derives a key of the configured size from the pass phrase.
    /// </summary>
    /// <param name="passPhrase">The secret to derive the key from.</param>
    /// <returns>The derived key bytes, sized to match <see cref="KeySize"/>.</returns>
    private byte[] GetPasswordBytes(string passPhrase)
    {
        var password = new PasswordDeriveBytes(passPhrase, null);
        byte[] keyBytes = password.GetBytes((int)_keysize / 8);

        return keyBytes;
    }

    /// <summary>
    /// Builds an AES encryptor from the key derived from the pass phrase and the configured IV.
    /// </summary>
    /// <param name="passPhrase">The secret to derive the key from.</param>
    /// <returns>A transform that encrypts data.</returns>
    private ICryptoTransform CreateEncryptor(string passPhrase)
    {
        byte[] keyBytes = GetPasswordBytes(passPhrase);

        var encryptor = EncryptionAlgorithm.CreateEncryptor(keyBytes, InitVectorBytes);

        return encryptor;
    }

    /// <summary>
    /// Builds an AES decryptor from the key derived from the pass phrase and the configured IV.
    /// </summary>
    /// <param name="passPhrase">The secret to derive the key from.</param>
    /// <returns>A transform that decrypts data.</returns>
    private ICryptoTransform CreateDecryptor(string passPhrase)
    {
        byte[] keyBytes = GetPasswordBytes(passPhrase);

        ICryptoTransform encryptor = EncryptionAlgorithm.CreateDecryptor(keyBytes, InitVectorBytes);

        return encryptor;
    }

    /// <summary>
    /// Runs the raw bytes through an AES encryptor and returns the cipher bytes.
    /// </summary>
    /// <param name="passPhrase">The secret to derive the key from.</param>
    /// <param name="data">The clear bytes to encrypt.</param>
    /// <returns>The encrypted bytes.</returns>
    private byte[] Encrypt(string passPhrase, byte[] data)
    {
        ICryptoTransform encryptor = CreateEncryptor(passPhrase);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(data, 0, data.Length);
        cryptoStream.FlushFinalBlock();
        byte[] cipherTextBytes = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();

        return cipherTextBytes;
    }

    /// <summary>
    /// Runs the cipher bytes through an AES decryptor.
    /// </summary>
    /// <param name="passPhrase">The secret to derive the key from.</param>
    /// <param name="data">The encrypted bytes to decrypt.</param>
    /// <returns>The decrypted bytes together with the number of valid bytes recovered.</returns>
    private (byte[] data, int byteCount) Decrypt(string passPhrase, byte[] data)
    {
        ICryptoTransform decryptor = CreateDecryptor(passPhrase);

        MemoryStream memoryStream = new MemoryStream(data);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] plainTextBytes;

        var binaryReader = new BinaryReader(cryptoStream);

        plainTextBytes = binaryReader.ReadBytes(data.Length);

        binaryReader.Close();

        memoryStream.Close();
        cryptoStream.Close();

        return (plainTextBytes, plainTextBytes.Length);
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Encrypts a UTF-8 string and returns the cipher text as Base64.
    /// </summary>
    /// <param name="plainText">The clear text to encrypt.</param>
    /// <param name="passPhrase">The secret used to derive the key.</param>
    /// <returns>The Base64-encoded cipher text.</returns>
    public override string EncryptString(string plainText, string passPhrase)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] cipherTextBytes = Encrypt(passPhrase, plainTextBytes);

        return Convert.ToBase64String(cipherTextBytes);
    }

    /// <summary>
    /// Encrypts a raw byte buffer.
    /// </summary>
    /// <param name="data">The clear bytes to encrypt.</param>
    /// <param name="passPhrase">The secret used to derive the key.</param>
    /// <returns>The encrypted bytes.</returns>
    public override byte[] EncryptBytes(byte[] data, string passPhrase)
    {
        return Encrypt(passPhrase, data); ;
    }

    /// <summary>
    /// Decrypts Base64 cipher text produced by <see cref="EncryptString"/> back to clear text.
    /// </summary>
    /// <param name="cipherText">The Base64-encoded cipher text.</param>
    /// <param name="passPhrase">The secret used to derive the key; must match the one used to encrypt.</param>
    /// <returns>The recovered clear text.</returns>
    public override string DecryptString(string cipherText, string passPhrase)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

        var result = Decrypt(passPhrase, cipherTextBytes);

        return Encoding.UTF8.GetString(result.data, 0, result.byteCount);
    }

    /// <summary>
    /// Decrypts a byte buffer produced by <see cref="EncryptBytes"/> back to its original form.
    /// </summary>
    /// <param name="data">The encrypted bytes.</param>
    /// <param name="passPhrase">The secret used to derive the key; must match the one used to encrypt.</param>
    /// <returns>The recovered clear bytes.</returns>
    public override byte[] DecryptBytes(byte[] data, string passPhrase)
    {
        var result = Decrypt(passPhrase, data);

        return result.data;
    }

    #endregion
}
