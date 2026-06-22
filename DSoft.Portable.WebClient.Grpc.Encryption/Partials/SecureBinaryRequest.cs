using System;
using DSoft.Portable.WebClient.Encryption;
using Google.Protobuf;

namespace DSoft.Portable.WebClient.Grpc.Encryption;

/// <summary>
/// Hand-written half of the protobuf-generated <c>SecureBinaryRequest</c> message. Implements
/// <see cref="ISecureBinaryRequest{T, T2}"/> so a gRPC request can carry both an encrypted payload and an
/// encrypted binary blob (as a protobuf <see cref="ByteString"/>).
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureBinaryRequest{T, T2}" />
public partial class SecureBinaryRequest : ISecureBinaryRequest<SecurePayload, ByteString>
{
    /// <summary>
    /// Protobuf construction hook: gives the request a fresh, empty payload.
    /// </summary>
    partial void OnConstruction()
    {
        Payload = new SecurePayload();


    }

    /// <summary>
    /// Creates a request stamped with the client version and a pre-encrypted payload string.
    /// </summary>
    /// <param name="clientVersionNo">The version of the client making the request.</param>
    /// <param name="data">The already-encrypted payload string.</param>
    public SecureBinaryRequest(string clientVersionNo, string data) : this()
    {
        ClientVersionNo = clientVersionNo;

        Payload.Data = data;
    }

    /// <summary>
    /// Creates a request stamped with the client version, a pre-encrypted payload, and a correlation id.
    /// </summary>
    /// <param name="clientVersionNo">The version of the client making the request.</param>
    /// <param name="data">The already-encrypted payload string.</param>
    /// <param name="id">The correlation identifier for the request.</param>
    public SecureBinaryRequest(string clientVersionNo, string data, string id) : this(clientVersionNo, data)
    {
        Id = id;
    }

    /// <summary>
    /// Ensures the request carries a payload that is present and still within the allowed age.
    /// </summary>
    /// <param name="timeout">The maximum age the payload may have.</param>
    /// <exception cref="System.Exception">Thrown when there is no payload, or the payload is unstamped or older than <paramref name="timeout"/>.</exception>
    public void Validate(TimeSpan timeout)
    {
        if (Payload == null)
            throw new Exception("No payload in request");

        if (Payload.Timestamp == null || !Payload.Validate(timeout))
            throw new Exception("Payload has timed out");
    }

    /// <summary>
    /// Decrypts the request's payload and deserializes it into <typeparamref name="TData"/>.
    /// </summary>
    /// <typeparam name="TData">The type the encrypted payload represents.</typeparam>
    /// <param name="passKey">The pass phrase the payload was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The decrypted, deserialized payload.</returns>
    /// <exception cref="System.Exception">Thrown when the request has no payload to extract.</exception>
    public TData ExtractPayload<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        if (Payload == null)
            throw new Exception("No data");

        return Payload.Extract<TData>(passKey, initVector, keySize);
    }

    /// <summary>
    /// Encrypts a byte buffer and stores it as the request's binary content.
    /// </summary>
    /// <param name="data">The raw bytes to encrypt and attach.</param>
    /// <param name="passKey">The pass phrase to encrypt with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    public void SetBinaryObject(byte[] data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        BinaryObject = ByteString.CopyFrom(EncryptionProviderFactory.Build(initVector, keySize).EncryptBytes(data, passKey));
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

        return EncryptionProviderFactory.Build(initVector, keySize).DecryptBytes(BinaryObject.ToByteArray(), passKey);
    }
}
