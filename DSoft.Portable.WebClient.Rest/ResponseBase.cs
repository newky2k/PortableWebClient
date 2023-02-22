using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest
{
	/// <summary>
	/// Class ResponseBase.
	/// </summary>
	public abstract class ResponseBase
    {
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ResponseBase"/> is success.
		/// </summary>
		/// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
		public bool Success { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseBase"/> class.
		/// </summary>
		public ResponseBase()
        {
            Success = true;
        }
    }
}
