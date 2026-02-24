using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Exceptions
{
	/// <summary>
	/// Class NoServerResponseException.
	/// Implements the <see cref="Exception" />
	/// </summary>
	/// <seealso cref="Exception" />
	public class NoServerResponseException : Exception
    {
        private string _errorMessage;
		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <value>The message.</value>
		public override string Message
        {
            get
            {
                return $"Unable to connect - Message - {_errorMessage}";
            }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NoServerResponseException"/> class.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="exception">The exception.</param>
		public NoServerResponseException(string errorMessage, Exception exception) : base(null, exception)
        {
            _errorMessage = errorMessage;
        }
    }
}
