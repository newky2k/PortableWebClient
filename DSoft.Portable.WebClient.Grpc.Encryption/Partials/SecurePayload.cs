using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
	/// <summary>
	/// Class SecurePayload.
	/// Implements the <see cref="ISecurePayload" />
	/// Implements the <see cref="Google.Protobuf.IMessage{T}" />
	/// Implements the <see cref="Google.Protobuf.IBufferMessage" />
	/// </summary>
	/// <seealso cref="ISecurePayload" />
	/// <seealso cref="Google.Protobuf.IMessage{T}" />
	/// <seealso cref="Google.Protobuf.IBufferMessage" />
	public partial class SecurePayload : ISecurePayload
    {
		/// <summary>
		/// Called when [construction].
		/// </summary>
		partial void OnConstruction()
        {
            Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecurePayload"/> class.
		/// </summary>
		/// <param name="dataValue">The data value.</param>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public SecurePayload(object dataValue, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this()
        {
            Data = PayloadManager.EncryptPayload(dataValue, passKey, initVector, keySize);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecurePayload"/> class.
		/// </summary>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public SecurePayload(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(EmptyPayload.Empty, passKey, initVector, keySize)
        {

        }

		/// <summary>
		/// Validates the specified time span.
		/// </summary>
		/// <param name="timeSpan">The time span.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Validate(TimeSpan timeSpan)
        {
            var timeStamp = Timestamp.ToDateTime();

            var diff = DateTime.Now - timeStamp;

            return (diff < timeSpan);
        }

		/// <summary>
		/// Extracts the specified pass key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>T.</returns>
		public T Extract<T>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            return PayloadManager.DecryptPayload<T>(Data, passKey, initVector, keySize);
        }

        
    }
}
