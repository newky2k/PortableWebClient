using DSoft.Portable.WebClient;
using DSoft.Portable.WebClient.Rest;
using DSoft.Portable.WebClient.Rest.Encryption;
using PortableClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHarness.Client;

namespace TestHarness.Services
{
	public class SessionService : RestServiceClientBase
	{
		protected override string ControllerName => "Session";

        private TestApiClient TypedClient => (TestApiClient)this.WebClient;

		public SessionService(TestApiClient client) : base(client)
		{

		}

        public async Task<SessionDto> GenerateSessionTokenAsync()
        {
            var result = await ExecutePostRequestAsync<SecureResponse>("Create", () => new SecureRequest(TypedClient.PassKey, ClientVersionNo, DateTime.Now.ToUniversalTime(), TypedClient.IVKey));

            if (!result.Success)
            {
                throw new Exception(result.Message);
            }

            var session = result.Payload.Extract<SessionDto>(TypedClient.PassKey, TypedClient.IVKey);

            return session;
        }
    }
}
