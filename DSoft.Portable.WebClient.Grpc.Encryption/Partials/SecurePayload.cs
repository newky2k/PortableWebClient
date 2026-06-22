using System;
using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;

namespace DSoft.Portable.WebClient.Grpc.Encryption;

/// <summary>
/// Hand-written half of the protobuf-generated <c>SecurePayload</c> message. Implements
/// <see cref="ISecurePayload"/> so the gRPC payload can be encrypted, time-stamped, age-checked, and decrypted
/// just like its REST counterpart.
/// </summary>
/// <seealso cref="ISecurePayload" />
public partial class SecurePayload : ISecurePayload
{
    /// <summary>
    /// Protobuf construction hook: stamps the message with the current UTC time so freshness can be enforced.
    /// </summary>
    partial void OnConstruction()
    {
        Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow);
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

    /// <summary>
    /// Checks whether the payload is still within its allowed age, comparing its protobuf timestamp to now.
    /// </summary>
    /// <param name="timeSpan">The maximum age the payload may have.</param>
    /// <returns><c>true</c> when the payload is younger than <paramref name="timeSpan"/>; otherwise <c>false</c>.</returns>
    public bool Validate(TimeSpan timeSpan)
    {
        var timeStamp = Timestamp.ToDateTime();

        var diff = DateTime.Now - timeStamp;

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


}
