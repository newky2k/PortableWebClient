using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Core.Exceptions
{
    /// <summary>
    /// Cookies were invalid
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InvalidCookiesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCookiesException"/> class.
        /// </summary>
        public InvalidCookiesException() : base("Cookies are invalid or missing")
        {

        }
    }
}
