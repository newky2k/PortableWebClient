using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest
{
	/// <summary>
	/// Class RequestBase.
	/// </summary>
	public abstract class RequestBase
    {
		/// <summary>
		/// Gets or sets the client version no.
		/// </summary>
		/// <value>The client version no.</value>
		public string ClientVersionNo { get; set; }
    }
}
