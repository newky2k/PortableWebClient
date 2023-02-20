using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient
{
	/// <summary>
	/// IWebClient Interface
	/// Extends the <see cref="IDisposable" />
	/// </summary>
	/// <seealso cref="IDisposable" />
	public interface IWebClient : IDisposable
    {
		/// <summary>
		/// Gets the base URL.
		/// </summary>
		/// <value>The base URL.</value>
		string BaseUrl { get; }

		/// <summary>
		/// Gets the client version no.
		/// </summary>
		/// <value>The client version no.</value>
		string ClientVersionNo { get; }

		/// <summary>
		/// Gets a value indicating whether this instance can connect.
		/// </summary>
		/// <value><c>true</c> if this instance can connect; otherwise, <c>false</c>.</value>
		bool CanConnect { get; }

		/// <summary>
		/// Gets the time out.
		/// </summary>
		/// <value>The time out.</value>
		int TimeOut { get; }

    }
}
