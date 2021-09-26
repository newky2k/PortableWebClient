using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
    /// <summary>
    /// Secure base service class
    /// </summary>
    /// <seealso cref="DSoft.Portable.WebClient.Rest.RestServiceClientBase" />
    public abstract class RestServiceSecureClientBase : RestServiceClientBase
    {
        public string InitVector => ((ISecureWebClient)WebClient).InitVector;

        public KeySize KeySize => ((ISecureWebClient)WebClient).KeySize;

        protected RestServiceSecureClientBase(ISecureWebClient client) : base(client)
        {

        }
    }

    /// <summary>
    /// Secure base service class
    /// </summary>
    /// <seealso cref="DSoft.Portable.WebClient.Rest.RestServiceClientBase" />
    public abstract class RestServiceSecureClientBase<T> : RestServiceSecureClientBase where T : ISecureWebClient
    {
        protected T Client => (T)WebClient;

        protected RestServiceSecureClientBase(T client) : base(client)
        {

        }
    }
}
