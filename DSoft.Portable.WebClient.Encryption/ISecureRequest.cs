using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Interface ISecureRequest
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISecureRequest<T> where T : ISecurePayload
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		string Id { get; set; }

		/// <summary>
		/// Gets or sets the token identifier.
		/// </summary>
		/// <value>The token identifier.</value>
		string TokenId { get; set; }

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		string Context { get; set; }

		/// <summary>
		/// Gets or sets the payload.
		/// </summary>
		/// <value>The payload.</value>
		T Payload { get; set; }

		/// <summary>
		/// Validates the specified timeout.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		void Validate(TimeSpan timeout);

		/// <summary>
		/// Extracts the payload.
		/// </summary>
		/// <typeparam name="TData">The type of the t data.</typeparam>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>TData.</returns>
		TData ExtractPayload<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
    }
}
