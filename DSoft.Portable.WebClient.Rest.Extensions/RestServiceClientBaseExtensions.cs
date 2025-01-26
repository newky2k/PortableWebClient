using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Rest.Encryption;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Rest
{
	/// <summary>
	/// RestServiceClientBaseExtensions.
	/// </summary>
	public static class RestServiceClientBaseExtensions
    {
        #region Rest Request Builders


        /// <summary>
        /// Builds the user post request.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="action">The action.</param>
        /// <param name="data">The data.</param>
        /// <param name="tokenId">The token identifier.</param>
        /// <param name="encryptionToken">The encryption token.</param>
        /// <param name="headers">Optional custom headers.</param>
        /// <returns>RestRequest.</returns>
        public static RestRequest BuildUserPostRequest(this RestServiceSecureClientBase target, string action, object data, string tokenId, string encryptionToken, Dictionary<string, string> headers = null)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action, parameterString:null), Method.Post);

            target.ApplyHeaders(request, headers);

            request.AddJsonBody(target.CreateUserRequest(data, tokenId, encryptionToken));

            return request;
        }

        /// <summary>
        /// Builds the empty user post request.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="action">The action.</param>
        /// <param name="tokenId">The token identifier.</param>
        /// <param name="encryptionToken">The encryption token.</param>
        /// <param name="headers">Optional custom headers.</param>
        /// <returns>RestRequest.</returns>
        public static RestRequest BuildEmptyUserPostRequest(this RestServiceSecureClientBase target, string action, string tokenId, string encryptionToken, Dictionary<string, string> headers = null)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action, parameterString: null), Method.Post);

            target.ApplyHeaders(request, headers);

            request.AddJsonBody(target.CreateEmptyUserRequest(tokenId, encryptionToken));

            return request;
        }

        /// <summary>
        /// Builds the user binary post request.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="action">The action.</param>
        /// <param name="data">The data.</param>
        /// <param name="binary">The binary.</param>
        /// <param name="tokenId">The token identifier.</param>
        /// <param name="encryptionToken">The encryption token.</param>
        /// <param name="headers">Optional custom headers.</param>
        /// <returns>RestRequest.</returns>
        public static RestRequest BuildUserBinaryPostRequest(this RestServiceSecureClientBase target, string action, object data, byte[] binary, string tokenId, string encryptionToken, Dictionary<string, string> headers = null)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action, parameterString: null), Method.Post);

            target.ApplyHeaders(request, headers);

            request.AddJsonBody(target.CreateUserBinaryRequest(data, binary, tokenId, encryptionToken));

            return request;
        }

        /// <summary>
        /// Builds the secure post request.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="action">The action.</param>
        /// <param name="data">The data.</param>
        /// <param name="tokenId">The token identifier.</param>
        /// <param name="encryptionToken">The encryption token.</param>
        /// <param name="headers">Optional custom headers.</param>
        /// <returns>RestRequest.</returns>
        public static RestRequest BuildSecurePostRequest(this RestServiceSecureClientBase target, string action, object data, string tokenId, string encryptionToken, Dictionary<string, string> headers = null)
        {
            var request = new RestRequest(target.CalculateUrlForMethod(action, parameterString: null), Method.Post);

            target.ApplyHeaders(request, headers);

            var fReq = new SecureRequest()
            {
                ClientVersionNo = target.Options.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(data, encryptionToken, target.InitVector, target.KeySize),
            };

            request.AddJsonBody(fReq);

            return request;
        }

		#endregion

		#region SecureRequest Builders


		/// <summary>
		/// Creates the empty user request.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="tokenId">The token identifier.</param>
		/// <param name="encryptionToken">The encryption token.</param>
		/// <returns>SecureRequest.</returns>
		public static SecureRequest CreateEmptyUserRequest(this RestServiceSecureClientBase target, string tokenId, string encryptionToken)
        {
            return new SecureRequest()
            {
                ClientVersionNo = target.Options.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(encryptionToken, target.InitVector, target.KeySize),
            };
        }

		/// <summary>
		/// Creates the user request.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="data">The data.</param>
		/// <param name="tokenId">The token identifier.</param>
		/// <param name="encryptionToken">The encryption token.</param>
		/// <returns>SecureRequest.</returns>
		public static SecureRequest CreateUserRequest(this RestServiceSecureClientBase target, object data, string tokenId, string encryptionToken)
        {
            return new SecureRequest()
            {
                ClientVersionNo = target.Options.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(data, encryptionToken, target.InitVector, target.KeySize),
            };
        }

		/// <summary>
		/// Creates the user binary request.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="data">The data.</param>
		/// <param name="binary">The binary.</param>
		/// <param name="tokenId">The token identifier.</param>
		/// <param name="encryptionToken">The encryption token.</param>
		/// <returns>SecureBinaryRequest.</returns>
		public static SecureBinaryRequest CreateUserBinaryRequest(this RestServiceSecureClientBase target, object data, byte[] binary, string tokenId, string encryptionToken)
        {
            var request = new SecureBinaryRequest()
            {
                ClientVersionNo = target.Options.ClientVersionNo,
                Id = tokenId,
                Payload = new SecurePayload(data, encryptionToken, target.InitVector, target.KeySize),
            };

            request.SetBinaryObject(binary, encryptionToken, target.InitVector, target.KeySize);

            return request;
        }

		#endregion

		#region Request execution methods

		/// <summary>
		/// Execute secure call as an asynchronous operation.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="data">The data.</param>
		/// <param name="tokenId">The token identifier.</param>
		/// <param name="encryptionToken">The encryption token.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		/// <exception cref="System.Exception"></exception>
		public static async Task ExecuteSecureCallAsync(this RestServiceSecureClientBase target, string methodName, object data, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken);

            target.ApplyHeaders(request);

            var result = await target.ExecutePostAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);
        }

		/// <summary>
		/// Execute secure call as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target">The target.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="tokenId">The token identifier.</param>
		/// <param name="encryptionToken">The encryption token.</param>
		/// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
		/// <exception cref="System.Exception"></exception>
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

		/// <summary>
		/// Execute secure call as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target">The target.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="data">The data.</param>
		/// <param name="tokenId">The token identifier.</param>
		/// <param name="encryptionToken">The encryption token.</param>
		/// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
		/// <exception cref="System.Exception"></exception>
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

		/// <summary>
		/// Execute secure binary call as an asynchronous operation.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="data">The data.</param>
		/// <param name="binary">The binary.</param>
		/// <param name="tokenId">The token identifier.</param>
		/// <param name="encryptionToken">The encryption token.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		/// <exception cref="System.Exception"></exception>
		public static async Task ExecuteSecureBinaryCallAsync(this RestServiceSecureClientBase target, string methodName, object data, byte[] binary, string tokenId, string encryptionToken)
        {
            var request = target.BuildUserBinaryPostRequest(methodName, data, binary, tokenId, encryptionToken);

            target.ApplyHeaders(request);

            var result = await target.ExecuteRequestAsync<SecureResponse>(request);

            if (result.Success == false)
                throw new Exception(result.Message);
        }

		/// <summary>
		/// Execute secure download service call as an asynchronous operation.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="data">The data.</param>
		/// <param name="tokenId">The token identifier.</param>
		/// <param name="encryptionToken">The encryption token.</param>
		/// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
		/// <exception cref="System.Exception"></exception>
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

