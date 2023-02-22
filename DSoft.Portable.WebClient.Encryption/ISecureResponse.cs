using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Interface ISecureResponse
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISecureResponse<T> where T : ISecurePayload
    {
		/// <summary>
		/// Gets or sets the payload.
		/// </summary>
		/// <value>The payload.</value>
		T Payload { get; set; }

		/// <summary>
		/// Sets the payload.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		void SetPayload(object data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);

		/// <summary>
		/// Sets the payload.
		/// </summary>
		/// <param name="data">The data.</param>
		void SetPayload(string data);

		/// <summary>
		/// Extracts the specified pass key.
		/// </summary>
		/// <typeparam name="TData">The type of the t data.</typeparam>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>TData.</returns>
		TData Extract<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
    }
}
