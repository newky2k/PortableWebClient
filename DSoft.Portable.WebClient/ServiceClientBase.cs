using DSoft.Portable.WebClient.Core.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient
{

    /// <summary>
    /// Base Service Client  class for consuming services provided by ASP.NET ApiControllers
    /// </summary>
    public abstract class ServiceClientBase : IDisposable
    {
        #region Fields
        private WebClientBase _client;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the rest client.
        /// </summary>
        /// <value>
        /// The rest client.
        /// </value>
        public RestClient RestClient
        {
            get
            {
                return new RestClient { BaseUrl = new Uri(_client.BaseUrl) };
            }
        }

        /// <summary>
        /// Gets the name of the Web Api Controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        public abstract string ControllerName { get; }

        /// <summary>
        /// Optional api module name if the api has been modularized  /api/module/controller
        /// </summary>
        /// <value>The module.</value>
        public virtual string Module
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the API prefix inserted before the controller name E.g. api/controller/method
        /// </summary>
        /// <value>
        /// The api prefix - default: api
        /// </value>
        public virtual string ApiPrefix => "api";

        #endregion



        public ServiceClientBase(WebClientBase client)
        {
            _client = client;

        }

        #region Methods

        public WebClientBase WebClient
        {
            get
            {
                return _client;
            }
        }

        /// <summary>
        /// Access a typed version of the client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        protected T Client<T>() where T : WebClientBase
        {
            return (T)_client;
        }

        /// <summary>
        /// Calculates the URL for method based on BaseUrl of the Client, the controller name and the method name
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        public string CalculateUrlForMethod(string methodName)
        {
            var apiPrefix = ApiPrefix;

            if (!string.IsNullOrEmpty(apiPrefix) && !apiPrefix.EndsWith(@"/"))
            {
                apiPrefix = $"{apiPrefix}/";
            }

            var url = $"{apiPrefix}{ControllerName}/{methodName}";

            if (!string.IsNullOrWhiteSpace(Module))
            {
                url = $"{apiPrefix}{Module}/{ControllerName}/{methodName}";
            }

            return url;
        }

        /// <summary>
        /// Builds a Post Request for the method 
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="type">The data format.</param>
        /// <returns></returns>
        protected RestRequest BuildPostRequest(string methodName, DataFormat type = DataFormat.Json)
        {
            return new RestRequest(CalculateUrlForMethod(methodName), Method.POST, type);
        }

        /// <summary>
        /// Builds a Get Request for the method
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="type">The data format</param>
        /// <returns></returns>
        protected RestRequest BuildGetRequest(string methodName, DataFormat type = DataFormat.Json)
        {
            return new RestRequest(CalculateUrlForMethod(methodName), Method.GET, type);
        }

        /// <summary>
        /// Execute a Request asynchronously
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="request">Request</param>
        /// <returns></returns>
        public async Task<T> ExecuteRequestAsync<T>(IRestRequest request) where T : ResponseBase
        {
            var result = await RestClient.ExecuteAsync<T>(request);

            if (!result.IsSuccessful)
            {
                if (result.StatusCode == 0)
                    throw new NoServerResponseException(result.StatusCode, result.ErrorMessage, result.ErrorException);
                else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception(result.StatusDescription);
            }

            if (result.Data.Success == false)
                throw new Exception(result.Data.Message);

            return result.Data;
        }

        /// <summary>
        /// Execute a Post request asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionName">Controller action name</param>
        /// <param name="packetBuilder">Function to buile request body object</param>
        /// <returns></returns>
        public async Task<T> ExecutePostRequestAsync<T>(string actionName, Func<object> packetBuilder) where T : ResponseBase
        {
            var request = BuildPostRequest(actionName);

            var body = packetBuilder?.Invoke();

            if (body != null)
                request.AddJsonBody(body);

            var result = await RestClient.ExecuteAsync<T>(request);

            if (!result.IsSuccessful)
            {
                if (result.StatusCode == 0)
                    throw new NoServerResponseException(result.StatusCode, result.ErrorMessage, result.ErrorException);

               else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception(result.StatusDescription);
            }

            if (!result.Data.Success)
            {
                throw new Exception(result.Data.Message);
            }

            return result.Data;
        }

        public virtual void Dispose()
        {
            _client = null;
        }

        #endregion

    }
}
