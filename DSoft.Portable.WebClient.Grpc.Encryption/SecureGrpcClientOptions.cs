using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
    /// <summary>
    /// Secure Grpc Client Options.
    /// Implements the <see cref="DSoft.Portable.WebClient.Grpc.GrpcClientOptions" />
    /// </summary>
    /// <seealso cref="DSoft.Portable.WebClient.Grpc.GrpcClientOptions" />
    public class SecureGrpcClientOptions : GrpcClientOptions
    {
        /// <summary>
        /// Gets or sets the size of the key for the encryption
        /// </summary>
        /// <value>The size of the key.</value>
        public KeySize KeySize { get; }
    }
}
