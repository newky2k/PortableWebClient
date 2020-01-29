using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Core.Exceptions
{
    public class NoServerResponseException : Exception
    {
        public NoServerResponseException() : base("Unable to connect to the server")
        {

        }
    }
}
