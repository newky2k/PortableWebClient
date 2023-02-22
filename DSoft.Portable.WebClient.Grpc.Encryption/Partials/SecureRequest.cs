using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
	/// <summary>
	/// Class SecureRequest.
	/// Implements the <see cref="DSoft.Portable.WebClient.Encryption.ISecureRequest{T}" />
	/// Implements the <see cref="Google.Protobuf.IMessage{T}" />
	/// Implements the <see cref="Google.Protobuf.IBufferMessage" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureRequest{T}" />
	/// <seealso cref="Google.Protobuf.IMessage{T}" />
	/// <seealso cref="Google.Protobuf.IBufferMessage" />
	public partial class SecureRequest : ISecureRequest<SecurePayload>
    {
		/// <summary>
		/// Called when [construction].
		/// </summary>
		partial void OnConstruction()
        {
            Payload = new SecurePayload();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureRequest"/> class.
		/// </summary>
		/// <param name="clientVersionNo">The client version no.</param>
		/// <param name="data">The data.</param>
		public SecureRequest(string clientVersionNo, string data) : this()
        {
            ClientVersionNo = clientVersionNo;

            Payload.Data = data;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureRequest"/> class.
		/// </summary>
		/// <param name="clientVersionNo">The client version no.</param>
		/// <param name="data">The data.</param>
		/// <param name="id">The identifier.</param>
		public SecureRequest(string clientVersionNo, string data, string id) : this(clientVersionNo, data)
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
    }
}
