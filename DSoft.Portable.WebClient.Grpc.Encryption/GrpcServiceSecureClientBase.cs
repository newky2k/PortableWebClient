using DSoft.Portable.WebClient.Encryption;
using Grpc.Core;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
    /// <summary>
    /// Secure ServiceClientBase.
    /// Implements the <see cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
    /// </summary>
    /// <seealso cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
    public abstract class GrpcServiceSecureClientBase : GrpcServiceClientBase
    {
		#region Fields
		private readonly IIVKeyProvider _initVectorProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the initialize vector.
        /// </summary>
        /// <value>The initialize vector.</value>
        public string InitVector => _initVectorProvider.InitVector;

		/// <summary>
		/// Gets the size of the key.
		/// </summary>
		/// <value>The size of the key.</value>
		public KeySize KeySize => ((SecureGrpcClientOptions)Options).KeySize;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceSecureClientBase"/> class.
        /// </summary>
        /// <param name="grpcChannelManager">The GRPC channel manager.</param>
        /// <param name="options">The options.</param>
        /// <param name="initVectorProvider">The initialize vector provider.</param>
        protected GrpcServiceSecureClientBase(IGrpcChannelManager grpcChannelManager, SecureGrpcClientOptions options, IIVKeyProvider initVectorProvider) : base(grpcChannelManager, options)
        {
            _initVectorProvider = initVectorProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceSecureClientBase"/> class.
        /// </summary>
        /// <param name="grpcChannelManager">The GRPC channel manager.</param>
        /// <param name="options">The options.</param>
        /// <param name="initVectorProvider">The initialize vector provider.</param>
        protected GrpcServiceSecureClientBase(IGrpcChannelManager grpcChannelManager, IOptions<SecureGrpcClientOptions> options, IIVKeyProvider initVectorProvider) : base(grpcChannelManager, options)
        {
            _initVectorProvider = initVectorProvider;
        }

        #endregion
    }

}
