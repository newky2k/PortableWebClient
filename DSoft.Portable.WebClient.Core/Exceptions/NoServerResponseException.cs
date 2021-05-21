using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DSoft.Portable.WebClient.Core.Exceptions
{
    public class NoServerResponseException : Exception
    {
        private string _errorMessage;
        public override string Message
        {
            get
            {
                return $"Unable to connect - Message - {_errorMessage}";
            }
        }

        public NoServerResponseException(string errorMessage, Exception exception) : base(null, exception)
        {
            _errorMessage = errorMessage;
        }
    }
}
