using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
	/// <summary>
	/// Class SecureResponse.
	/// Implements the <see cref="DSoft.Portable.WebClient.Encryption.ISecureResponse{T}" />
	/// Implements the <see cref="Google.Protobuf.IMessage{T}" />
	/// Implements the <see cref="Google.Protobuf.IBufferMessage" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureResponse{T}" />
	/// <seealso cref="Google.Protobuf.IMessage{T}" />
	/// <seealso cref="Google.Protobuf.IBufferMessage" />
	public partial class SecureResponse : ISecureResponse<SecurePayload>
    {

		/// <summary>
		/// Called when [construction].
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
		/// Sets the payload.
		/// </summary>
		/// <param name="data">The data.</param>
		public void SetPayload(string data)
        {
            Payload.Data = data;
        }

		/// <summary>
		/// Sets the payload.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public void SetPayload(object data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) => SetPayload(PayloadManager.EncryptPayload(data, passKey, initVector, keySize));

		/// <summary>
		/// Extracts the specified pass key.
		/// </summary>
		/// <typeparam name="TData">The type of the t data.</typeparam>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>TData.</returns>
		/// <exception cref="System.Exception">No data</exception>
		public TData Extract<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (Payload == null)
                throw new Exception("No data");

            return Payload.Extract<TData>(passKey, initVector, keySize);
        }

    }
}
