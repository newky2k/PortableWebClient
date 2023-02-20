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
        private GrpcChannel _RPCChannel;
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
        /// <value>
        /// The RPC channel.
        /// </value>
        internal protected GrpcChannel RPCChannel
        {
            get
            {
                if (_RPCChannel == null)
                {
					switch (_options.GrpcMode)
					{
						case HttpMode.Http_1_1:
							{
								//if a custom validator has be provided use that
								if (_options.ServerCertificateCustomValidationCallback != null)
								{
									var httpClientHandlerCustom = new HttpClientHandler();
									httpClientHandlerCustom.ServerCertificateCustomValidationCallback = _options.ServerCertificateCustomValidationCallback;

									_RPCChannel = GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions
									{
										HttpHandler = new GrpcWebHandler(httpClientHandlerCustom)
									});
								}
								else if (_options.DisableSSLCertValidation)
								{
									//return channel with SSL cert validation disabled
									var httpClientHandler = new HttpClientHandler();
#if NET6_0_OR_GREATER
									httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#else
                                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
#endif

									_RPCChannel = GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions
									{
										HttpHandler = new GrpcWebHandler(httpClientHandler)
									});
								}
								else
								{
									//if disable SSL certifiate validation has not been set them return standard channel generator
									_RPCChannel = GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions
									{
										HttpHandler = new GrpcWebHandler(new HttpClientHandler())
									});
								}
								break;
							}
						case HttpMode.Http_2_0:
							{
								//if a custom validator has be provided use that
								if (_options.ServerCertificateCustomValidationCallback != null)
								{
									var httpClientHandlerCustom = new HttpClientHandler();
									httpClientHandlerCustom.ServerCertificateCustomValidationCallback = _options.ServerCertificateCustomValidationCallback;

									_RPCChannel = GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions() { HttpClient = new HttpClient(httpClientHandlerCustom) });
								}
								else if (_options.DisableSSLCertValidation)
								{
									//return channel with SSL cert validation disabled
									var httpClientHandler = new HttpClientHandler();
#if NET6_0_OR_GREATER
									httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#else
                                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
#endif
									var httpClient = new HttpClient(httpClientHandler);

									_RPCChannel = GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions() { HttpClient = httpClient });
								}
								else
								{
									//if disable SSL certifiate validation has not been set them return standard channel generator
									_RPCChannel = GrpcChannel.ForAddress(_client.BaseUrl);
								}
								break;
							}
						default:
							throw new Exception("Unexpected HTTP mode for Grpc Channel");



					}
				}

				return _RPCChannel;

			}
        }

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
		protected GrpcServiceClientBase(IWebClient client, HttpMode httpMode = HttpMode.Http_1_1, bool disableSSLCertCheck = false) : this(client, new GrpcClientOptions() { GrpcMode = httpMode, DisableSSLCertValidation = disableSSLCertCheck })
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceClientBase"/> class.
        /// </summary>
        /// <param name="client">The web client.</param>
        /// <param name="options">GrpcClient options client</param>
        protected GrpcServiceClientBase(IWebClient client, GrpcClientOptions options)
        {
            
            if (client == null)
                throw new ArgumentNullException("client");

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
