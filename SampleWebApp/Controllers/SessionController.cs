using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

                //try and extract the empty payload, will fail if the key is wrong
                var payload = ExtractPayload<EmptyPayLoad>(request, initKey);

                await Task.Delay(10);

                result.Payload.Data = PayloadManager.EncryptPayload(EmptyPayLoad.Empty, initKey);



            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;

            }

            return result;
        }

        public static T ExtractPayload<T>(SecureRequest request, string encryptionToken)
        {
            var payLoad = request.Payload.Extract<T>(encryptionToken);

            return payLoad;
        }
    }
}