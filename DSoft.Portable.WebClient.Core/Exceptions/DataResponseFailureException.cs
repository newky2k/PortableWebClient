using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Core.Exceptions
{
    public class DataResponseFailureException : Exception
    {
        public DataResponseFailureException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
