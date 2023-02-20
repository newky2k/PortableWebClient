using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
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
        private IWebClient _client;
        private GrpcClientOptions _options;

		private readonly IGrpcChannelManager _grpcChannelManager;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the web client.
		/// </summary>
		/// <value>The web client.</value>
		protected IWebClient WebClient => _client;


		/// <summary>
		/// Gets the RPC channel.
		/// </summary>
		/// <value>The RPC channel.</value>
		/// <exception cref="System.Exception">Unexpected HTTP mode for Grpc Channel</exception>
		internal protected GrpcChannel RPCChannel => _grpcChannelManager.ForAddress(_client.BaseUrl, _options);

		/// <summary>
		/// Gets the client version no.
		/// </summary>
		/// <value>The client version no.</value>
		internal protected string ClientVersionNo => _client.ClientVersionNo;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="GrpcServiceClientBase"/> class.
		/// </summary>
		/// <param name="client">The web client.</param>
		/// <param name="httpMode">The HTTP mode.</param>
		/// <param name="disableSSLCertCheck">Disable SSL cert validation</param>
		protected GrpcServiceClientBase(IWebClient client, HttpMode httpMode = HttpMode.Http_1_1, bool disableSSLCertCheck = false) : this(client, new GrpcChannelManager(), new GrpcClientOptions() { GrpcMode = httpMode, DisableSSLCertValidation = disableSSLCertCheck })
        {

        }

		/// <summary>
		/// Initializes a new instance of the <see cref="GrpcServiceClientBase"/> class.
		/// </summary>
		/// <param name="client">The web client.</param>
		/// <param name="options">GrpcClient options client</param>
		protected GrpcServiceClientBase(IWebClient client, GrpcClientOptions options) : this(client, new GrpcChannelManager(), options)
		{

		}


		/// <summary>
		/// Initializes a new instance of the <see cref="GrpcServiceClientBase"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="grpcChannelManager">The GRPC channel manager.</param>
		/// <param name="options">GrpcClient options client</param>
		/// <exception cref="System.ArgumentNullException">client</exception>
		protected GrpcServiceClientBase(IWebClient client, IGrpcChannelManager grpcChannelManager, GrpcClientOptions options)
		{

			if (client == null)
				throw new ArgumentNullException("client");

			_grpcChannelManager = grpcChannelManager;

			_client = client;

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
	/// <typeparam name="T2">The type of the t2.</typeparam>
	/// <seealso cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
	public abstract class GrpcServiceClientBase<T, T2> : GrpcServiceClientBase 
        where T : ClientBase
        where T2 : IWebClient
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
		/// Initializes a new instance of the <see cref="GrpcServiceClientBase{T, T2}"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		protected GrpcServiceClientBase(T2 client) : base(client)
        {

        }

    }
}
