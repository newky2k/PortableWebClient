using DSoft.Portable.WebClient;
using DSoft.Portable.WebClient.Grpc;
using Microsoft.Extensions.Options;
using SampleRpc;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SampleApiClient
{
	public class SampleServiceClient : GrpcServiceClientBase
	{
        #region Fields
        private Version _appVersion;
        #endregion

        /// <summary>
        /// Gets the client version no, from the assemblt
        /// </summary>
        /// <value>
        /// The client version no.
        /// </value>
        protected override string ClientVersionNo
        {
            get
            {
                if (_appVersion == null)
                {
                    var asm = Assembly.GetAssembly(this.GetType());

                    _appVersion = asm.GetName().Version;
                }


                return _appVersion.ToString();
            }
        }

        public SampleServiceClient(IGrpcChannelManager channelManager, IOptions<GrpcClientOptions> options) : base(channelManager, options)
        {

        }

        public SampleServiceClient(IGrpcChannelManager channelManager, GrpcClientOptions options) : base(channelManager, options)
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