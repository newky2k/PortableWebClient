using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
	/// <summary>
	/// Class SecureResponse.
	/// Implements the <see cref="DSoft.Portable.WebClient.Rest.ResponseBase" />
	/// Implements the <see cref="DSoft.Portable.WebClient.Encryption.ISecureResponse{T}" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Rest.ResponseBase" />
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureResponse{T}" />
	public class SecureResponse : ResponseBase, ISecureResponse<SecurePayload>
    {
		/// <summary>
		/// Gets or sets the payload.
		/// </summary>
		/// <value>The payload.</value>
		public SecurePayload Payload { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureResponse"/> class.
		/// </summary>
		public SecureResponse()
        {
            Success = true;

            Payload = new SecurePayload();
        }

		/// <summary>
		/// Sets the payload.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <exception cref="System.Exception">Payload is not set</exception>
		public void SetPayload(string data)
        {
            if (Payload == null)
                throw new Exception("Payload is not set");

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
