using DSoft.Portable.WebClient;
using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Rest;
using DSoft.Portable.WebClient.Rest.Encryption;
using PortableClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness.Services
{
	public class SessionService : RestServiceSecureClientBase
    {
		protected override string ControllerName => "Session";

        public override string ClientVersionNo => "1.0";


        public SessionService(SecureRestApiClientOptions options, IIVKeyProvider initVectorProvider) : base(options, initVectorProvider)
        {

		}

        public async Task<SessionDto> GenerateSessionTokenAsync(string passKey)
        {
            var result = await ExecutePostAsync<SecureResponse>("Create", () => new SecureRequest(passKey, ClientVersionNo, DateTime.Now.ToUniversalTime(), InitVector, KeySize));

            if (!result.Success)
            {
                throw new Exception(result.Message);
            }

            var session = result.Payload.Extract<SessionDto>(passKey, InitVector, KeySize);

            return session;
        }
    }
}
