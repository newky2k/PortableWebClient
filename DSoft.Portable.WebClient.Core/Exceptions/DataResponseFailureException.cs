using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Core.Exceptions
{
	/// <summary>
	/// Class DataResponseFailureException.
	/// Implements the <see cref="Exception" />
	/// </summary>
	/// <seealso cref="Exception" />
	public class DataResponseFailureException : Exception
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DataResponseFailureException"/> class.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		public DataResponseFailureException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
