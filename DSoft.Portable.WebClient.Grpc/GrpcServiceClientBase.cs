using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace DSoft.Portable.WebClient.Grpc
{
	/// <summary>
	/// Base Grpc Service class
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public abstract class GrpcServiceClientBase : IDisposable
	{
		#region Fields
		private GrpcClientOptions _options;
		private readonly IGrpcChannelManager _grpcChannelManager;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the options provided to the client
        /// </summary>
        /// <value>The options.</value>
        protected GrpcClientOptions Options => _options;

        /// <summary>
        /// Gets the RPC channel.
        /// </summary>
        /// <value>The RPC channel.</value>
        /// <exception cref="System.Exception">Unexpected HTTP mode for Grpc Channel</exception>
        protected GrpcChannel RPCChannel => _grpcChannelManager.ForAddress(_options.UrlBuilder(), _options);

        /// <summary>
        /// Get the GRPC channel manager.
        /// </summary>
        /// <value>The GRPC channel manager.</value>
        protected IGrpcChannelManager GrpcChannelManager => _grpcChannelManager;

        /// <summary>
        /// Gets the client version no.
        /// </summary>
        /// <value>The client version no.</value>
        internal protected string ClientVersionNo => _options.ClientVersionNo;

		#endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceClientBase"/> class.
        /// </summary>
        /// <param name="grpcChannelManager">The GRPC channel manager.</param>
        /// <param name="options">The options.</param>
        protected GrpcServiceClientBase(IGrpcChannelManager grpcChannelManager, IOptions<GrpcClientOptions> options) : this(grpcChannelManager, options.Value)
        {

		}
        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceClientBase"/> class.
        /// </summary>
        /// <param name="grpcChannelManager">The GRPC channel manager.</param>
        /// <param name="options">GrpcClient options client</param>
        /// <exception cref="System.ArgumentNullException">client</exception>
        protected GrpcServiceClientBase(IGrpcChannelManager grpcChannelManager, GrpcClientOptions options)
		{
			_grpcChannelManager = grpcChannelManager;


			//set default options
			_options = (options == null) ? new GrpcClientOptions() : options;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public virtual void Dispose()
        {
            
        }
    }

	/// <summary>
	/// Base Grpc Service class generics.
	/// Implements the <see cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
	public abstract class GrpcServiceClientBase<T> : GrpcServiceClientBase 
        where T : ClientBase
    {

		/// <summary>
		/// Gets the typed client.
		/// </summary>
		/// <value>The client.</value>
		protected T Client
        {
            get
            {
                var rpcClient = Activator.CreateInstance(typeof(T), new object[] { RPCChannel });

                return (T)rpcClient;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceClientBase{T}"/> class.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceClientBase"/> class.
        /// </summary>
        /// <param name="grpcChannelManager">The GRPC channel manager.</param>
        /// <param name="options">The options.</param>
        protected GrpcServiceClientBase(IGrpcChannelManager grpcChannelManager, IOptions<GrpcClientOptions> options) : base(grpcChannelManager, options.Value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceClientBase"/> class.
        /// </summary>
        /// <param name="grpcChannelManager">The GRPC channel manager.</param>
        /// <param name="options">GrpcClient options client</param>
        /// <exception cref="System.ArgumentNullException">client</exception>
        protected GrpcServiceClientBase(IGrpcChannelManager grpcChannelManager, GrpcClientOptions options) : base(grpcChannelManager, options) { }

    }
}
