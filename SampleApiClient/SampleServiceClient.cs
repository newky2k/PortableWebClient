using DSoft.Portable.WebClient;
using DSoft.Portable.WebClient.Grpc;
using SampleRpc;
using System;
using System.Threading.Tasks;

namespace SampleApiClient
{
	public class SampleServiceClient : GrpcServiceClientBase
	{


		public SampleServiceClient(IWebClient client, IGrpcChannelManager channelManager,  HttpMode httpMode = HttpMode.Http_1_1, bool disableSSLCertCheck = false) : base(client, channelManager, httpMode, disableSSLCertCheck)
		{

		}

		public async Task<SimpleResponse> FindAsync(int id)
		{

			var rpcClient = new SampleRpc.SampleContract.SampleContractClient(RPCChannel);

			var result = await rpcClient.FindAsync(new SampleRpc.SimpleRequest()
			{
				Id = "1",
			});




			return result;
		}
	}
}