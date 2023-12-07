﻿using DSoft.Portable.WebClient;
using DSoft.Portable.WebClient.Grpc;
using Microsoft.Extensions.Options;
using SampleRpc;
using System;
using System.Threading.Tasks;

namespace SampleApiClient
{
	public class SampleServiceClient : GrpcServiceClientBase
	{


		public SampleServiceClient(IWebClient client, IGrpcChannelManager channelManager,  HttpMode httpMode, bool disableSSLCertCheck) : base(client, channelManager, httpMode, disableSSLCertCheck)
		{

        }//

        public SampleServiceClient(IWebClient client, IGrpcChannelManager channelManager, IOptions<GrpcClientOptions> options) : base(client, channelManager, options)
        {

        }

        public async Task<SimpleResponse> FindAsync(int id)
		{

			var rpcClient = new SampleRpc.SampleContract.SampleContractClient(RPCChannel);

			var result = await rpcClient.FindAsync(new SampleRpc.SimpleRequest()
			{
				Id = id.ToString(),
			});


			return result;
		}
	}
}