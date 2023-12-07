using DSoft.Portable.WebClient;
using DSoft.Portable.WebClient.Grpc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApiClient
{
	public class SampleWebClient : WebClientBase
	{
		public SampleWebClient(string baseUrl) : base(baseUrl)
		{

		}

		public SampleWebClient(IOptions<GrpcClientOptions> options) : this(options.Value) 
		{

		}

        public SampleWebClient(GrpcClientOptions options) : base(options.UrlBuilder())
        {

        }

    }
}
