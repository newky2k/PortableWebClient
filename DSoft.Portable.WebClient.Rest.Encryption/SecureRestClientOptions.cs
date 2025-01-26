using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
    /// <summary>
    /// Security Options for the rest client
    /// </summary>
    public class SecureRestClientOptions : RestClientOptions
    {
        /// <summary>
        /// Gets or sets the size of the key for the encryption
        /// </summary>
        /// <value>The size of the key.</value>
        public KeySize KeySize { get; set; }
    }
}
