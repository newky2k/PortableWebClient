using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest
{
    public abstract class ResponseBase
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public ResponseBase()
        {
            Success = true;
        }
    }
}
