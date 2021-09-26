using DSoft.Portable.WebClient.Encryption;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
    public abstract class GrpcServiceSecureClientBase : GrpcServiceClientBase
    {
        public string InitVector => ((ISecureWebClient)WebClient).InitVector;

        public KeySize KeySize => ((ISecureWebClient)WebClient).KeySize;


        protected GrpcServiceSecureClientBase(ISecureWebClient client, HttpMode httpMode = HttpMode.Http_1_1) : base(client, httpMode)
        {

        }

    }

    public abstract class GrpcServiceClientBase<T, T2> : GrpcServiceClientBase
        where T : ClientBase
        where T2 : ISecureWebClient
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
