using DSoft.Portable.WebClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApiClient
{
	public class WebClient : WebClientBase
	{
		public WebClient(string baseUrl) : base(baseUrl)
		{

		}
	}
}
