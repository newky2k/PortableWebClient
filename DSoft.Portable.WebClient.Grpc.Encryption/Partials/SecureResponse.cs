using System;
using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;

namespace DSoft.Portable.WebClient.Grpc.Encryption;

/// <summary>
/// Hand-written half of the protobuf-generated <c>SecureResponse</c> message. Implements
/// <see cref="ISecureResponse{T}"/> so the gRPC response envelope can populate and decrypt its encrypted
/// <see cref="SecurePayload"/> like its REST counterpart.
/// </summary>
/// <seealso cref="ISecureResponse{T}" />
public partial class SecureResponse : ISecureResponse<SecurePayload>
{

    /// <summary>
    /// Protobuf construction hook: gives the response an empty payload stamped with the current UTC time.
    /// </summary>
    partial void OnConstruction()
    {
        //Success = true;

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
