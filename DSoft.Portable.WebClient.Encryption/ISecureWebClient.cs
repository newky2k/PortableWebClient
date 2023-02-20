using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Interface ISecureWebClient
	/// Extends the <see cref="DSoft.Portable.WebClient.IWebClient" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.IWebClient" />
	public interface ISecureWebClient : IWebClient
    {
		/// <summary>
		/// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
		/// </summary>
		/// <value>The initialize vector.</value>
		string InitVector { get; }


		/// <summary>
		/// Gets or sets the size of the key for the encryption
		/// </summary>
		/// <value>The size of the key.</value>
		KeySize KeySize { get; }
    }
}
