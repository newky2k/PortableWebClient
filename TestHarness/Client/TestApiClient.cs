using DSoft.Portable.WebClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHarness.Services;

namespace TestHarness.Client
{
	public class TestApiClient : WebClientBase
	{
		internal string PassKey => "1234567890";
		internal string IVKey => "xRFg8Ctp1sEqWfVp";

		public SessionService Session => new SessionService(this);

		public TestApiClient(string baseUrl) : base(baseUrl)
		{

		}
	}
}
