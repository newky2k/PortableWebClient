using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
	/// <summary>
	/// Class SecureBinaryResponse.
	/// Implements the <see cref="DSoft.Portable.WebClient.Rest.Encryption.SecureResponse" />
	/// Implements the <see cref="DSoft.Portable.WebClient.Encryption.ISecureBinaryResponse{T, T2}" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Rest.Encryption.SecureResponse" />
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureBinaryResponse{T, T2}" />
	public class SecureBinaryResponse : SecureResponse, ISecureBinaryResponse<SecurePayload, byte[]>
    {
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets the type of the MIME.
		/// </summary>
		/// <value>The type of the MIME.</value>
		public string MimeType { get; set; }

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public byte[] Data { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureBinaryResponse"/> class.
		/// </summary>
		public SecureBinaryResponse()
        {

        }
    }
}
