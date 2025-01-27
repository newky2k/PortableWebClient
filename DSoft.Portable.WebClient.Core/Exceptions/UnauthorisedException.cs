using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Core.Exceptions
{
    /// <summary>
    /// Thrown when unauthorised 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class UnauthorisedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorisedException"/> class.
        /// </summary>
        public UnauthorisedException() : base("You do not have access to this rsesource")
        {
            
        }
    }
}
