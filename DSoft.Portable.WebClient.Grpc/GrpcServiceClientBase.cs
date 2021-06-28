using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System;
using System.Net.Http;

namespace DSoft.Portable.WebClient.Grpc
{
    public abstract class GrpcServiceClientBase : IDisposable
    {
        private IWebClient _client;

        protected IWebClient WebClient => _client;

        /// <summary>
        /// Gets the RPC channel.
        /// </summary>
        /// <value>
        /// The RPC channel.
        /// </value>
        protected GrpcChannel RPCChannel => GrpcChannel.ForAddress(_client.BaseUrl, new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(new HttpClientHandler())
        });

        internal protected string ClientVersionNo => _client.ClientVersionNo;

        protected GrpcServiceClientBase(IWebClient client)
        {
            _client = client;
        }

        public void Dispose()
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
