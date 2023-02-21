using Grpc.Core;
using SampleRpc;
using System.Threading.Tasks;

namespace SampleWebApp.Services
{
	public class SampleServiceRpc : SampleRpc.SampleContract.SampleContractBase
	{

		public SampleServiceRpc()
		{

		}

		public override Task<SimpleResponse> Find(SimpleRequest request, ServerCallContext context)
		{
			return Task.FromResult(new SimpleResponse()
			{
				Success = true,
				Message = "Hello, World!",
			});
		}
	}
}
