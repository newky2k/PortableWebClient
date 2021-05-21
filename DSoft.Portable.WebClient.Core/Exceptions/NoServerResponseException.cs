using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DSoft.Portable.WebClient.Core.Exceptions
{
    public class NoServerResponseException : Exception
    {
        private HttpStatusCode _httpStatusCode;
        private string _errorMessage;
        public override string Message
        {
            get
            {
                return $"Unable to connect - Status: {_httpStatusCode} - Message - {_errorMessage}";
            }
        }

        public NoServerResponseException(HttpStatusCode httpStatusCode, string errorMessage, Exception exception) : base(null, exception)
        {
            _httpStatusCode = httpStatusCode;
            _errorMessage = errorMessage;
        }
    }
}
