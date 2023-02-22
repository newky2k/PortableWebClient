using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using System;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
	/// <summary>
	/// Class SecurePayload.
	/// Implements the <see cref="ISecurePayload" />
	/// </summary>
	/// <seealso cref="ISecurePayload" />
	public class SecurePayload : ISecurePayload
    {
		#region Properties

		/// <summary>
		/// Gets or sets the timestamp.
		/// </summary>
		/// <value>The timestamp.</value>
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public string Data { get; set; }


		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SecurePayload"/> class.
		/// </summary>
		public SecurePayload()
        {
            Timestamp = DateTime.Now;
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

		#endregion

		#region Methods

		/// <summary>
		/// Validates the specified time span.
		/// </summary>
		/// <param name="timeSpan">The time span.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Validate(TimeSpan timeSpan)
        {
            var diff = DateTime.Now - Timestamp;

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

        #endregion
    }

    
}
