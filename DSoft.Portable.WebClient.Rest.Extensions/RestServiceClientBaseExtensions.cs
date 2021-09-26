using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Rest.Encryption;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Rest
{
    public static class RestServiceClientBaseExtensions
    {
        #region Rest Request Builders


        public static RestRequest BuildUserPostRequest(this RestServiceSecureClientBase target, string action, object data, string tokenId, string encryptionToken)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action), Method.POST, DataFormat.Json);

            target.ApplyHeaders(request);

            request.AddJsonBody(target.CreateUserRequest(data, tokenId, encryptionToken));

            return request;
        }

        public static RestRequest BuildEmptyUserPostRequest(this RestServiceSecureClientBase target, string action, string tokenId, string encryptionToken)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action), Method.POST, DataFormat.Json);

            target.ApplyHeaders(request);

            request.AddJsonBody(target.CreateEmptyUserRequest(tokenId, encryptionToken));

            return request;
        }

        public static RestRequest BuildUserBinaryPostRequest(this RestServiceSecureClientBase target, string action, object data, byte[] binary, string tokenId, string encryptionToken)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action), Method.POST, DataFormat.Json);

            target.ApplyHeaders(request);

            request.AddJsonBody(target.CreateUserBinaryRequest(data, binary, tokenId, encryptionToken));

            return request;
        }

        public static RestRequest BuildSecurePostRequest(this RestServiceSecureClientBase target, string action, object data, string tokenId, string encryptionToken)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action), Method.POST, DataFormat.Json);

            target.ApplyHeaders(request);

            var fReq = new SecureRequest()
            {
                ClientVersionNo = target.WebClient.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(data, encryptionToken, target.InitVector, target.KeySize),
            };

            request.AddJsonBody(fReq);

            return request;
        }

        #endregion

        #region SecureRequest Builders


        public static SecureRequest CreateEmptyUserRequest(this RestServiceSecureClientBase target, string tokenId, string encryptionToken)
        {
            return new SecureRequest()
            {
                ClientVersionNo = target.WebClient.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(encryptionToken, target.InitVector, target.KeySize),
            };
        }

        public static SecureRequest CreateUserRequest(this RestServiceSecureClientBase target, object data, string tokenId, string encryptionToken)
        {
            return new SecureRequest()
            {
                ClientVersionNo = target.WebClient.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(data, encryptionToken, target.InitVector, target.KeySize),
            };
        }

        public static SecureBinaryRequest CreateUserBinaryRequest(this RestServiceSecureClientBase target, object data, byte[] binary, string tokenId, string encryptionToken)
        {
            var request = new SecureBinaryRequest()
            {
                ClientVersionNo = target.WebClient.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(data, encryptionToken, target.InitVector, target.KeySize),
            };

            request.SetBinaryObject(binary, encryptionToken, target.InitVector, target.KeySize);

            return request;
        }

        #endregion

        #region Request execution methods

        public static async Task ExecuteSecureCallAsync(this RestServiceSecureClientBase target, string methodName, object data, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken);

            target.ApplyHeaders(request);

            var result = await target.ExecuteRequestAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);
        }

        public static async Task<T> ExecuteSecureCallAsync<T>(this RestServiceSecureClientBase target, string methodName, string tokenId, string encryptionToken)
        {
            var request = target.BuildEmptyUserPostRequest(methodName, tokenId, encryptionToken);

            target.ApplyHeaders(request);

            var result = await target.ExecuteRequestAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);

            var payload = result.Payload.Extract<T>(encryptionToken, target.InitVector, target.KeySize);

            return payload;
        }

        public static async Task<T> ExecuteSecureCallAsync<T>(this RestServiceSecureClientBase target, string methodName, object data, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken);

            target.ApplyHeaders(request);

            var result = await target.ExecuteRequestAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);

            var payload = result.Payload.Extract<T>(encryptionToken, target.InitVector, target.KeySize);

            return payload;
        }

        public static async Task ExecuteSecureBinaryCallAsync(this RestServiceSecureClientBase target, string methodName, object data, byte[] binary, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserBinaryPostRequest(methodName, data, binary, tokenId, encryptionToken);

            target.ApplyHeaders(request);

            var result = await target.ExecuteRequestAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);
        }

        public static async Task<(string FileName, string MimeType, byte[] Binary)> ExecuteSecureDownloadServiceCallAsync(this RestServiceSecureClientBase target, string methodName, object data, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken);

            target.ApplyHeaders(request);

            var result = await target.ExecuteRequestAsync<SecureBinaryResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);

            return (result.FileName, result.MimeType, result.Data);
        }

        #endregion
    }
}

