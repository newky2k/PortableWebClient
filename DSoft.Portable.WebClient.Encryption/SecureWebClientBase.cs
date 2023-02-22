using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Class SecureWebClientBase.
	/// Implements the <see cref="DSoft.Portable.WebClient.WebClientBase" />
	/// Implements the <see cref="DSoft.Portable.WebClient.Encryption.ISecureWebClient" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.WebClientBase" />
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureWebClient" />
	public abstract class SecureWebClientBase : WebClientBase, ISecureWebClient
    {
		/// <summary>
		/// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
		/// </summary>
		/// <value>The initialize vector.</value>
		public abstract string InitVector { get; }

		/// <summary>
		/// Gets or sets the size of the key for the encryption
		/// </summary>
		/// <value>The size of the key.</value>
		public abstract KeySize KeySize { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureWebClientBase"/> class.
		/// </summary>
		/// <param name="baseUrl">The base URL.</param>
		protected SecureWebClientBase(string baseUrl) : base(baseUrl)
        {
        }


    }
}
