using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DSoft.Portable.WebClient.Core.Exceptions
{
	/// <summary>
	/// Class ServerResponseFailureException.
	/// Implements the <see cref="Exception" />
	/// </summary>
	/// <seealso cref="Exception" />
	public class ServerResponseFailureException : Exception
    {
        private HttpStatusCode _httpStatusCode;
        private string _errorMessage;
		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <value>The message.</value>
		public override string Message
        {
            get
            {
                return $"Response failure - Status: {_httpStatusCode} - Message - {_errorMessage}";
            }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerResponseFailureException"/> class.
		/// </summary>
		/// <param name="httpStatusCode">The HTTP status code.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="exception">The exception.</param>
		public ServerResponseFailureException(HttpStatusCode httpStatusCode, string errorMessage, Exception exception) : base(null, exception)
        {
            _httpStatusCode = httpStatusCode;
            _errorMessage = errorMessage;
        }
    }
}
