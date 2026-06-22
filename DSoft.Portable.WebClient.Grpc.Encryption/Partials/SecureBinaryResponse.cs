using System;
using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using Google.Protobuf;

namespace DSoft.Portable.WebClient.Grpc.Encryption;

/// <summary>
/// Hand-written half of the protobuf-generated <c>SecureBinaryResponse</c> message. Implements
/// <see cref="ISecureBinaryResponse{T, T2}"/> so a gRPC response can return an encrypted file as a protobuf
/// <see cref="ByteString"/> alongside its encrypted payload.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureBinaryResponse{T, T2}" />
public partial class SecureBinaryResponse : ISecureBinaryResponse<SecurePayload, ByteString>
{
    /// <summary>
    /// The binary content as a plain byte array, or null when no content is set.
    /// </summary>
    byte[] AsByteArray => Data?.ToByteArray();

    /// <summary>
    /// Protobuf construction hook: gives the response an empty payload stamped with the current UTC time.
    /// </summary>
    partial void OnConstruction()
    {
        Payload = new SecurePayload()
        {
            Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
        };
    }

    /// <summary>
    /// Stores an already-encrypted string as the response payload.
    /// </summary>
    /// <param name="data">The pre-encrypted payload string.</param>
    public void SetPayload(string data)
    {
        Payload.Data = data;
    }

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

    /// <summary>
    /// Encrypts an object and stores it as the response payload.
    /// </summary>
    /// <param name="data">The object to encrypt and return.</param>
    /// <param name="passKey">The pass phrase to encrypt with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public void SetPayload(object data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) => SetPayload(PayloadManager.EncryptPayload(data, passKey, initVector, keySize));


}
