using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
	/// <summary>
	/// Class SecureBinaryRequest.
	/// Implements the <see cref="DSoft.Portable.WebClient.Encryption.ISecureBinaryRequest{T, T2}" />
	/// Implements the <see cref="Google.Protobuf.IMessage{T}" />
	/// Implements the <see cref="IBufferMessage" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureBinaryRequest{T, T2}" />
	/// <seealso cref="Google.Protobuf.IMessage{T}" />
	/// <seealso cref="IBufferMessage" />
	public partial class SecureBinaryRequest : ISecureBinaryRequest<SecurePayload, ByteString>
    {
		/// <summary>
		/// Called when [construction].
		/// </summary>
		partial void OnConstruction()
        {
            Payload = new SecurePayload();

            
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureBinaryRequest"/> class.
		/// </summary>
		/// <param name="clientVersionNo">The client version no.</param>
		/// <param name="data">The data.</param>
		public SecureBinaryRequest(string clientVersionNo, string data) : this()
        {
            ClientVersionNo = clientVersionNo;

            Payload.Data = data;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureBinaryRequest"/> class.
		/// </summary>
		/// <param name="clientVersionNo">The client version no.</param>
		/// <param name="data">The data.</param>
		/// <param name="id">The identifier.</param>
		public SecureBinaryRequest(string clientVersionNo, string data, string id) : this(clientVersionNo, data)
        {
            Id = id;
        }

		/// <summary>
		/// Validates the specified timeout.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <exception cref="System.Exception">No payload in request</exception>
		/// <exception cref="System.Exception">Payload has timed out</exception>
		public void Validate(TimeSpan timeout)
        {
            if (Payload == null)
                throw new Exception("No payload in request");

            if (Payload.Timestamp == null || !Payload.Validate(timeout))
                throw new Exception("Payload has timed out");
        }

		/// <summary>
		/// Extracts the payload.
		/// </summary>
		/// <typeparam name="TData">The type of the t data.</typeparam>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>TData.</returns>
		/// <exception cref="System.Exception">No data</exception>
		public TData ExtractPayload<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (Payload == null)
                throw new Exception("No data");

            return Payload.Extract<TData>(passKey, initVector, keySize);
        }

		/// <summary>
		/// Sets the binary object.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public void SetBinaryObject(byte[] data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            BinaryObject = ByteString.CopyFrom(EncryptionProviderFactory.Build(initVector, keySize).EncryptBytes(data, passKey));
        }

		/// <summary>
		/// Gets the binary object.
		/// </summary>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>System.Byte[].</returns>
		/// <exception cref="System.Exception">Binary Object is empty</exception>
		public byte[] GetBinaryObject(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (BinaryObject == null)
                throw new Exception("Binary Object is empty");

            return EncryptionProviderFactory.Build(initVector, keySize).DecryptBytes(BinaryObject.ToByteArray(), passKey);
        }
    }
}
