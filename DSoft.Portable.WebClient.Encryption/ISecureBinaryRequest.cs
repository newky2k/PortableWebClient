using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Interface ISecureBinaryRequest
	/// Extends the <see cref="DSoft.Portable.WebClient.Encryption.ISecureRequest{T}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="T2">The type of the t2.</typeparam>
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.ISecureRequest{T}" />
	public interface ISecureBinaryRequest<T, T2> : ISecureRequest<T> where T : ISecurePayload
    {
		/// <summary>
		/// Gets or sets the binary object.
		/// </summary>
		/// <value>The binary object.</value>
		T2 BinaryObject { get; set; }

		/// <summary>
		/// Sets the binary object.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		void SetBinaryObject(byte[] data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);

		/// <summary>
		/// Gets the binary object.
		/// </summary>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>System.Byte[].</returns>
		byte[] GetBinaryObject(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
    }
}
