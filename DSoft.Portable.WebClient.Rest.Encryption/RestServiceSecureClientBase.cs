using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
	/// <summary>
	/// Secure base service class
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Rest.RestServiceClientBase" />
	public abstract class RestServiceSecureClientBase : RestServiceClientBase
    {
		/// <summary>
		/// Gets the initialize vector.
		/// </summary>
		/// <value>The initialize vector.</value>
		public string InitVector => ((ISecureWebClient)WebClient).InitVector;

		/// <summary>
		/// Gets the size of the key.
		/// </summary>
		/// <value>The size of the key.</value>
		public KeySize KeySize => ((ISecureWebClient)WebClient).KeySize;

		/// <summary>
		/// Initializes a new instance of the <see cref="RestServiceSecureClientBase"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		protected RestServiceSecureClientBase(ISecureWebClient client) : base(client)
        {

        }
    }

	/// <summary>
	/// Secure base service class
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="DSoft.Portable.WebClient.Rest.RestServiceClientBase" />
	public abstract class RestServiceSecureClientBase<T> : RestServiceSecureClientBase where T : ISecureWebClient
    {
		/// <summary>
		/// Gets the client.
		/// </summary>
		/// <value>The client.</value>
		protected T Client => (T)WebClient;

		/// <summary>
		/// Initializes a new instance of the <see cref="RestServiceSecureClientBase{T}"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		protected RestServiceSecureClientBase(T client) : base(client)
        {

        }
    }
}
