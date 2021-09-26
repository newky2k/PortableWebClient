using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public abstract class SecureWebClientBase : WebClientBase, ISecureWebClient
    {
        public abstract string InitVector { get; }

        public abstract KeySize KeySize { get; }

        protected SecureWebClientBase(string baseUrl) : base(baseUrl)
        {
        }


    }
}
