using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
	/// <summary>
	/// Class SecureRequest.
	/// Implements the <see cref="DSoft.Portable.WebClient.Rest.RequestBase" />
	/// Implements the <see cref="DSoft.Portable.WebClient.Encryption.ISecureRequest{T}" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Rest.RequestBase" />
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureRequest{T}" />
	public class SecureRequest : RequestBase, ISecureRequest<SecurePayload>
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the token identifier.
		/// </summary>
		/// <value>The token identifier.</value>
		public string TokenId { get; set; }

		/// <summary>
		/// Gets or sets the payload.
		/// </summary>
		/// <value>The payload.</value>
		public SecurePayload Payload { get; set; }

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		public string Context { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureRequest"/> class.
		/// </summary>
		public SecureRequest()
        {
            Payload = new SecurePayload();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureRequest"/> class.
		/// </summary>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public SecureRequest(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            Payload = new SecurePayload(passKey, initVector, keySize);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureRequest"/> class.
		/// </summary>
		/// <param name="passKey">The pass key.</param>
		/// <param name="data">The data.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public SecureRequest(string passKey, object data, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            Payload = new SecurePayload(data, passKey, initVector, keySize);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureRequest"/> class.
		/// </summary>
		/// <param name="passKey">The pass key.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public SecureRequest(string passKey, DateTime timestamp, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(passKey, initVector, keySize)
        {
            Payload.Timestamp = timestamp;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureRequest"/> class.
		/// </summary>
		/// <param name="passKey">The pass key.</param>
		/// <param name="clientVersionNumber">The client version number.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public SecureRequest(string passKey, string clientVersionNumber, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(passKey, initVector, keySize)
        {
            ClientVersionNo = clientVersionNumber;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureRequest"/> class.
		/// </summary>
		/// <param name="passKey">The pass key.</param>
		/// <param name="clientVersionNumber">The client version number.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public SecureRequest(string passKey, string clientVersionNumber, DateTime timestamp, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(passKey, clientVersionNumber, initVector, keySize)
        {
            Payload.Timestamp = timestamp;
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

            if (Payload.Timestamp == default || !Payload.Validate(timeout))
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
