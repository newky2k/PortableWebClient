using DSoft.Portable.WebClient.Core.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Rest
{

    /// <summary>
    /// Base Service Client  class for consuming services provided by ASP.NET ApiControllers
    /// </summary>
    public abstract class RestServiceClientBase : IDisposable
    {
        #region Fields
        private IWebClient _client;
        #endregion

        #region Properties

        protected string ClientVersionNo => WebClient.ClientVersionNo;

        /// <summary>
        /// Gets the rest client.
        /// </summary>
        /// <value>
        /// The rest client.
        /// </value>
        protected IRestClient RestClient
        {
            get
            {
                return new RestClient { BaseUrl = new Uri(_client.BaseUrl), Timeout = _client.TimeOut };
            }
        }

        /// <summary>
        /// Gets the name of the Web Api Controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        protected abstract string ControllerName { get; }

        /// <summary>
        /// Optional api module name if the api has been modularized  /api/module/controller
        /// </summary>
        /// <value>The module.</value>
        protected virtual string Module { get; }

        /// <summary>
        /// Gets the API prefix inserted before the controller name E.g. api/controller/method
        /// </summary>
        /// <value>
        /// The api prefix - default: api
        /// </value>
        protected virtual string ApiPrefix => "api";

        /// <summary>
        /// Returns a collection of custom headers.
        /// </summary>
        /// <value>
        /// The custom headers.
        /// </value>
        protected virtual ICollection<KeyValuePair<string, string>> CustomHeaders { get;  }

        /// <summary>
        /// Gets the WebCient instancee.
        /// </summary>
        /// <value>
        /// The web client.
        /// </value>
        public IWebClient WebClient => _client;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceClientBase"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public RestServiceClientBase(IWebClient client)
        {
            _client = client;

        }

        #endregion

        #region Methods

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
            var request = new RestRequest(CalculateUrlForMethod(methodName), Method.POST, type);

            
            request.Timeout = 60000;

            ApplyHeaders(request);

            return request;
        }

        /// <summary>
        /// Builds a Get Request for the method
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="type">The data format</param>
        /// <returns></returns>
        protected RestRequest BuildGetRequest(string methodName, DataFormat type = DataFormat.Json)
        {
            var request = new RestRequest(CalculateUrlForMethod(methodName), Method.GET, type);

            ApplyHeaders(request);

            return request;
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
                    throw new NoServerResponseException(result.ErrorMessage, result.ErrorException);
                else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ServerResponseFailureException(result.StatusCode, result.ErrorMessage, result.ErrorException);
            }

            if (result.Data.Success == false)
                throw new DataResponseFailureException(result.Data.Message);

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
                    throw new NoServerResponseException(result.ErrorMessage, result.ErrorException);

               else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ServerResponseFailureException(result.StatusCode, result.ErrorMessage, result.ErrorException);
            }

            if (!result.Data.Success)
            {
                throw new DataResponseFailureException(result.Data.Message);
            }

            return result.Data;
        }

        public virtual void Dispose()
        {
            _client = null;
        }

        public void ApplyHeaders(IRestRequest request)
        {
            if (CustomHeaders != null && CustomHeaders.Count > 0)
            {
                request.AddHeaders(CustomHeaders);
            }
        }
        #endregion

    }

    /// <summary>
    /// Generic Typed version of the ServiceClientBase type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.IDisposable" />
    public abstract class RestServiceClientBase<T> : RestServiceClientBase where T : IWebClient
    {
        protected T Client => (T)WebClient;

        protected RestServiceClientBase(T client) : base(client)
        {

        }
    } 

}
