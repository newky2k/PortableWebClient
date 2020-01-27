using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient
{
    public static class ServiceClientBaseExtensions
    {
        #region Rest Request Builders


        public static RestRequest BuildUserPostRequest(this ServiceClientBase target, string action, object data, string tokenId, string encryptionToken)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action), Method.POST, DataFormat.Json);

            request.AddJsonBody(target.CreateUserRequest(data,tokenId, encryptionToken));

            return request;
        }

        public static RestRequest BuildEmptyUserPostRequest(this ServiceClientBase target, string action, string tokenId, string encryptionToken)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action), Method.POST, DataFormat.Json);

            request.AddJsonBody(target.CreateEmptyUserRequest(tokenId, encryptionToken));

            return request;
        }

        public static RestRequest BuildUserBinaryPostRequest(this ServiceClientBase target, string action, object data, byte[] binary, string tokenId, string encryptionToken)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action), Method.POST, DataFormat.Json);

            request.AddJsonBody(target.CreateUserBinaryRequest(data, binary,tokenId, encryptionToken));

            return request;
        }

        public static RestRequest BuildSecurePostRequest(this ServiceClientBase target, string action, object data, string tokenId, string encryptionToken)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action), Method.POST, DataFormat.Json);

            var fReq = new SecureRequest()
            {
                ClientVersionNo = target.WebClient.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(data, encryptionToken),
            };

            request.AddJsonBody(fReq);

            return request;
        }

        #endregion

        #region SecureRequest Builders


        public static SecureRequest CreateEmptyUserRequest(this ServiceClientBase target, string tokenId, string encryptionToken)
        {
            return new SecureRequest()
            {
                ClientVersionNo = target.WebClient.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(encryptionToken),
            };
        }

        public static SecureRequest CreateUserRequest(this ServiceClientBase target, object data, string tokenId, string encryptionToken)
        {
            return new SecureRequest()
            {
                ClientVersionNo = target.WebClient.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(data, encryptionToken),
            };
        }

        public static SecureBinaryRequest CreateUserBinaryRequest(this ServiceClientBase target, object data, byte[] binary, string tokenId, string encryptionToken)
        {
            var request = new SecureBinaryRequest()
            {
                ClientVersionNo = target.WebClient.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(data, encryptionToken),
            };

            request.SetBinaryObject(binary, encryptionToken);

            return request;
        }

        #endregion

        #region Request execution methods

        public static async Task ExecuteSecureCallAsync(this ServiceClientBase target, string methodName, object data, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken);

            var result = await target.ExecuteRequestAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);
        }

        public static async Task<T> ExecuteSecureCallAsync<T>(this ServiceClientBase target, string methodName, string tokenId, string encryptionToken)
        {
            var request = target.BuildEmptyUserPostRequest(methodName, tokenId, encryptionToken);

            var result = await target.ExecuteRequestAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);

            var payload = result.Payload.Extract<T>(encryptionToken);

            return payload;
        }

        public static async Task<T> ExecuteSecureCallAsync<T>(this ServiceClientBase target, string methodName, object data, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken);

            var result = await target.ExecuteRequestAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);

            var payload = result.Payload.Extract<T>(encryptionToken);

            return payload;
        }

        public static async Task ExecuteSecureBinaryCallAsync(this ServiceClientBase target, string methodName, object data, byte[] binary, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserBinaryPostRequest(methodName, data, binary, tokenId, encryptionToken);

            var result = await target.ExecuteRequestAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);
        }

        public static async Task<(string FileName, string MimeType, byte[] Binary)> ExecuteSecureDownloadServiceCallAsync(this ServiceClientBase target, string methodName, object data, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken);

            var result = await target.ExecuteRequestAsync<SecureBinaryResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);

            return (result.FileName, result.MimeType, result.Data);
        }

        #endregion
    }
}
