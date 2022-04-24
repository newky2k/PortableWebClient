using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System;
using System.Net.Http;

namespace DSoft.Portable.WebClient.Grpc
{
    /// <summary>
    /// Base Grpc Service class
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class GrpcServiceClientBase : IDisposable
    {
        private IWebClient _client;


        private GrpcClientOptions _options;
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
                switch (_options.GrpcMode)
                {
                    case HttpMode.Http_1_1:
                        {
                            //if a custom validator has be provided use that
                            if (_options.ServerCertificateCustomValidationCallback != null)
                            {
                                var httpClientHandlerCustom = new HttpClientHandler();
                                httpClientHandlerCustom.ServerCertificateCustomValidationCallback = _options.ServerCertificateCustomValidationCallback;

                                return GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions
                                {
                                    HttpHandler = new GrpcWebHandler(httpClientHandlerCustom)
                                });
                            }


                            if (_options.DisableSSLCertValidation)
                            {
                                //return channel with SSL cert validation disabled
                                var httpClientHandler = new HttpClientHandler();
#if NET6_0_OR_GREATER
                                httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#else
                                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
#endif

                                return GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions
                                {
                                    HttpHandler = new GrpcWebHandler(httpClientHandler)
                                });
                            }


                            //if disable SSL certifiate validation has not been set them return standard channel generator
                            return GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions
                            {
                                HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                            });


                        }
                    case HttpMode.Http_2_0:
                        {
                            //if a custom validator has be provided use that
                            if (_options.ServerCertificateCustomValidationCallback != null)
                            {
                                var httpClientHandlerCustom = new HttpClientHandler();
                                httpClientHandlerCustom.ServerCertificateCustomValidationCallback = _options.ServerCertificateCustomValidationCallback;

                                return GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions() { HttpClient = new HttpClient(httpClientHandlerCustom) });
                            }


                            if (_options.DisableSSLCertValidation)
                            {
                                //return channel with SSL cert validation disabled
                                var httpClientHandler = new HttpClientHandler();
#if NET6_0_OR_GREATER
                                httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#else
                                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
#endif
                                var httpClient = new HttpClient(httpClientHandler);

                                return GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions() { HttpClient = httpClient });
                            }



                            //if disable SSL certifiate validation has not been set them return standard channel generator
                            return GrpcChannel.ForAddress(_client.BaseUrl);
                        }
                    default:
                        throw new Exception("Unexpected HTTP mode for Grpc Channel");



                }
            }
        }

        internal protected string ClientVersionNo => _client.ClientVersionNo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceClientBase"/> class.
        /// </summary>
        /// <param name="client">The web client.</param>
        /// <param name="httpMode">The HTTP mode.</param>
        /// <param name="disbaleSSLCertCheck">Disable SSL cert validation</param>
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

        public virtual void Dispose()
        {
            
        }
    }

    public abstract class GrpcServiceClientBase<T, T2> : GrpcServiceClientBase 
        where T : ClientBase
        where T2 : IWebClient
    {


        protected T Client
        {
            get
            {
                var rpcClient = Activator.CreateInstance(typeof(T), new object[] { RPCChannel });

                return (T)rpcClient;
            }
        }

        protected GrpcServiceClientBase(T2 client) : base(client)
        {

        }

    }
}
