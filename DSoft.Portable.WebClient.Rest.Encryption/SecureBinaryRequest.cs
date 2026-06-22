using System;
using DSoft.Portable.WebClient.Encryption;

namespace DSoft.Portable.WebClient.Rest.Encryption;


/// <summary>
/// A <see cref="SecureRequest"/> that additionally carries an encrypted binary blob, for uploading a
/// file or stream alongside the structured payload.
/// </summary>
public class SecureBinaryRequest : SecureRequest, ISecureBinaryRequest<SecurePayload, byte[]>
{

    /// <summary>
    /// The encrypted binary content attached to the request.
    /// </summary>
    public byte[] BinaryObject { get; set; }


    /// <summary>
    /// Encrypts a byte buffer and stores it as the request's binary content.
    /// </summary>
    /// <param name="data">The raw bytes to encrypt and attach.</param>
    /// <param name="passKey">The pass phrase to encrypt with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public void SetBinaryObject(byte[] data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        BinaryObject = EncryptionProviderFactory.Build(initVector, keySize).EncryptBytes(data, passKey);
    }

    /// <summary>
    /// Decrypts and returns the attached binary content.
    /// </summary>
    /// <param name="passKey">The pass phrase the content was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The decrypted bytes.</returns>
    /// <exception cref="System.Exception">Thrown when no binary content has been attached.</exception>
    public byte[] GetBinaryObject(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        if (BinaryObject == null)
            throw new Exception("Binary Object is empty");

        return EncryptionProviderFactory.Build(initVector, keySize).DecryptBytes(BinaryObject, passKey);
    }

    /// <summary>
    /// Creates an empty binary request; populate the payload and binary content before sending.
    /// </summary>
    public SecureBinaryRequest()
    {

    }
}
