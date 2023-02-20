using DSoft.Portable.WebClient.Encryption;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
	/// <summary>
	/// Class GrpcServiceSecureClientBase.
	/// Implements the <see cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
	public abstract class GrpcServiceSecureClientBase : GrpcServiceClientBase
    {
		/// <summary>
		/// Gets the initialize vector.
		/// </summary>
		/// <value>The initialize vector.</value>
		public string InitVector => ((ISecureWebClient)WebClient).InitVector;

		/// <summary>
		/// Gets the size of the key.
		/// </summary>
		/// <value>The size of the key.</value>
		public KeySize KeySize => ((ISecureWebClient)WebClient).KeySize;


		/// <summary>
		/// Initializes a new instance of the <see cref="GrpcServiceSecureClientBase"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="httpMode">The HTTP mode.</param>
		protected GrpcServiceSecureClientBase(ISecureWebClient client, HttpMode httpMode = HttpMode.Http_1_1) : base(client, httpMode)
        {

        }

    }

	/// <summary>
	/// GrpcServiceClientBase interface with Generics
	/// Implements the <see cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="T2">The type of the t2.</typeparam>
	/// <seealso cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
	public abstract class GrpcServiceClientBase<T, T2> : GrpcServiceClientBase
        where T : ClientBase
        where T2 : ISecureWebClient
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
