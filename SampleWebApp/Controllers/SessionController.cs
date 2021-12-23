using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using DSoft.Portable.WebClient.Rest.Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortableClient.Models;

namespace SampleWebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpPost]
        [ActionName("Create")]
        public async Task<SecureResponse> CreateSession([FromBody] SecureRequest request)
        {
            var result = new SecureResponse();

            try
            {
                //calculate the encryption Hash
                var initKey = "1234567890";
                var ivKey = "xRFg8Ctp1sEqWfVp";

                //try and extract the empty payload, will fail if the key is wrong
                var payload = ExtractPayload<EmptyPayload>(request, initKey, ivKey);

                await Task.Delay(10);

                var session = new SessionDto()
                {
                    Id = Guid.NewGuid(),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Timestamp = DateTime.UtcNow,
                    Token = Guid.NewGuid().ToString(),
                };

                result.Payload.Data = PayloadManager.EncryptPayload(session, initKey, ivKey);



            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;

            }

            return result;
        }

        public static T ExtractPayload<T>(SecureRequest request, string encryptionToken, string ivKey)
        {
            var payLoad = request.Payload.Extract<T>(encryptionToken, ivKey);

            return payLoad;
        }
    }
}