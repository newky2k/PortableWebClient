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
        private HttpMode _httpMode;

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
                switch (_httpMode)
                {
                    case HttpMode.Http_1_1:
                        {
                            return GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions
                            {
                                HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                            });
                        }
                    case HttpMode.Http_2_0:
                        {
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
        /// <param name="client">The client.</param>
        /// <param name="httpMode">The HTTP mode.</param>
        protected GrpcServiceClientBase(IWebClient client, HttpMode httpMode = HttpMode.Http_1_1)
        {
            _client = client;

            _httpMode = httpMode;
        }

        public virtual void Dispose()
        {
            
        }
    }

    public abstract class GrpcServiceClientBase<T> : GrpcServiceClientBase where T : ClientBase
    {


        protected T Client
        {
            get
            {
                var rpcClient = Activator.CreateInstance(typeof(T), new object[] { RPCChannel });

                return (T)rpcClient;
            }
        }

        protected GrpcServiceClientBase(IWebClient client) : base(client)
        {

        }

    }
}
