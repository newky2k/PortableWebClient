using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Interface ISecureBinaryResponse
	/// Extends the <see cref="DSoft.Portable.WebClient.Encryption.ISecureResponse{T}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="T2">The type of the t2.</typeparam>
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureResponse{T}" />
	public interface ISecureBinaryResponse<T,T2> : ISecureResponse<T> where T : ISecurePayload
    {
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		string FileName { get; set; }

		/// <summary>
		/// Gets or sets the type of the MIME.
		/// </summary>
		/// <value>The type of the MIME.</value>
		string MimeType { get; set; }

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		T2 Data { get; set; }
    }
}
