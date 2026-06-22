using System;
using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;

namespace DSoft.Portable.WebClient.Rest.Encryption;

/// <summary>
/// Concrete REST secure payload: an encrypted blob stamped with the time it was created, so the
/// receiver can both decrypt it and reject stale payloads.
/// </summary>
/// <seealso cref="ISecurePayload" />
public class SecurePayload : ISecurePayload
{
    #region Properties

    /// <summary>
    /// When the payload was created, used to enforce a freshness window on the receiving side.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The encrypted payload as a Base64-encoded string.
    /// </summary>
    public string Data { get; set; }


    #endregion

    #region Constructors

    /// <summary>
    /// Creates an empty payload stamped with the current time.
    /// </summary>
    public SecurePayload()
    {
        Timestamp = DateTime.Now;
    }

    /// <summary>
    /// Creates a payload by serializing and encrypting <paramref name="dataValue"/>.
    /// </summary>
    /// <param name="dataValue">The object to encrypt into the payload.</param>
    /// <param name="passKey">The pass phrase used to encrypt the payload.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public SecurePayload(object dataValue, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this()
    {
        Data = PayloadManager.EncryptPayload(dataValue, passKey, initVector, keySize);
    }

    /// <summary>
    /// Creates a payload that encrypts a random empty placeholder, for exchanges that carry no real data.
    /// </summary>
    /// <param name="passKey">The pass phrase used to encrypt the payload.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public SecurePayload(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(EmptyPayload.Empty, passKey, initVector, keySize)
    {

    }

    #endregion

    #region Methods

    /// <summary>
    /// Checks whether the payload is still within its allowed age.
    /// </summary>
    /// <param name="timeSpan">The maximum age the payload may have.</param>
    /// <returns><c>true</c> when the payload is younger than <paramref name="timeSpan"/>; otherwise <c>false</c>.</returns>
    public bool Validate(TimeSpan timeSpan)
    {
        var diff = DateTime.Now - Timestamp;

        return (diff < timeSpan);
    }

    /// <summary>
    /// Decrypts the payload and deserializes it into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type the encrypted payload represents.</typeparam>
    /// <param name="passKey">The pass phrase the payload was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The decrypted, deserialized payload.</returns>
    public T Extract<T>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        return PayloadManager.DecryptPayload<T>(Data, passKey, initVector, keySize);
    }

    #endregion
}
