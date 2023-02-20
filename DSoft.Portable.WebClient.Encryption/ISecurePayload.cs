using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Interface ISecurePayload
	/// </summary>
	public interface ISecurePayload
    {
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		string Data { get; set; }

		/// <summary>
		/// Validates the specified time span.
		/// </summary>
		/// <param name="timeSpan">The time span.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool Validate(TimeSpan timeSpan);

		/// <summary>
		/// Extracts the specified pass key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>T.</returns>
		T Extract<T>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
    }
}
