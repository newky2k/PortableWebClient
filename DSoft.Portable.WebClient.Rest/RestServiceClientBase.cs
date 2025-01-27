using DSoft.Portable.WebClient.Core;
using DSoft.Portable.WebClient.Core.Exceptions;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Rest
{

	/// <summary>
	/// Base Service Client  class for consuming services provided by ASP.NET ApiControllers
	/// </summary>
	public abstract class RestServiceClientBase
	{
		#region Fields
		private readonly RestClientOptions _options;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the options provided to the client
        /// </summary>
        /// <value>The options.</value>
        public RestClientOptions Options => _options;

        /// <summary>
        /// Gets the client version no.
        /// </summary>
        /// <value>The client version no.</value>
        protected string ClientVersionNo => _options.ClientVersionNo;

		/// <summary>
		/// Gets the rest client.
		/// </summary>
		/// <value>The rest client.</value>
		protected virtual IRestClient RestClient
		{
            get 
            {

                var options = new RestSharp.RestClientOptions()
                {
                    BaseUrl = _options.BaseUrl,
                    Timeout = _options.TimeOut,

                };

                if (_options.CookieContainer != null)
                {
                    options.CookieContainer = _options.CookieContainer;

                    return new RestClient(AuthenticatedClient, options) as IRestClient;
                }
                else
                {
                    return new RestClient(options) as IRestClient;
                }

            }
		}

        /// <summary>
        /// Gets and authenticated client with the custom cookie manager
        /// </summary>
        /// <value>
        /// The authenticated client.
        /// </value>
        protected HttpClient AuthenticatedClient
        {
            get
            {
                var handler = new HttpClientHandler { CookieContainer = _options.CookieContainer };
                var client = new HttpClient(handler);

                return client;
            }
        }

        /// <summary>
        /// Custom Cookie Manager
        /// </summary>
        protected ICookieManager CookieManager
        {
            get
            {
                if (_options.CookieContainer == null)
                    return null;

                if (_options.CookieContainer is ICookieManager cm)
                {
                    return cm;
                }

                return null;

            }
        }

        /// <summary>
        /// Gets the name of the Web Api Controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        protected abstract string ControllerName { get; }

		/// <summary>
		/// Optional api module name if the api has been modularized  /api/module/controller/method
		/// </summary>
		/// <value>The module.</value>
		protected virtual string Module { get; }

		/// <summary>
		/// Gets the API prefix inserted before the controller name E.g. api/controller/method
		/// </summary>
		/// <value>The api prefix - default: api</value>
		protected virtual string ApiPrefix => "";

        /// <summary>
        /// Gets the API prefix inserted before the controller name E.g. servicename/api/controller/method
        /// </summary>
        /// <value>The api prefix - default: api</value>
        protected virtual string ServiceName => "";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceClientBase"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        protected RestServiceClientBase(IOptions<RestClientOptions> options) : this(options.Value)
        {
				
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceClientBase"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        protected RestServiceClientBase(RestClientOptions options)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the URL for method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameterString">The parameter string.</param>
        /// <param name="controllerOverride">The controller override.</param>
        /// <returns></returns>
        public string CalculateUrlForMethod(string methodName, string parameterString = null, string controllerOverride = null)
		{
			var apiPrefix = ApiPrefix;

			if (!string.IsNullOrEmpty(apiPrefix) && !apiPrefix.EndsWith(@"/"))
			{
				apiPrefix = $"{apiPrefix}/";
			}

            var baseEndpointService = ApiPrefix;

            if (!string.IsNullOrWhiteSpace(ServiceName))
            {
                baseEndpointService = $"{apiPrefix}/{ServiceName}/";
            }

            var controllerComponent = ControllerName;

            if (!string.IsNullOrWhiteSpace(controllerOverride))
            {
                controllerComponent = controllerOverride;
            }

            var url = $"{baseEndpointService}{controllerComponent}/{methodName}";

			if (!string.IsNullOrWhiteSpace(Module))
			{
				url = $"{baseEndpointService}{Module}/{controllerComponent}/{methodName}";
			}

            if (!string.IsNullOrWhiteSpace(parameterString))
            {
                url = $"{url}{parameterString}";
            }

            return url;
		}

        /// <summary>
        /// Builds a Post Request for the method
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="controllerOverride">The controller override.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>
        /// RestRequest.
        /// </returns>
        protected RestRequest BuildPostRequest(string methodName, string controllerOverride = null, Dictionary<string, string> headers = null)
        {
            var request = new RestRequest(CalculateUrlForMethod(methodName, null, controllerOverride), Method.Post);

            ApplyHeaders(request, headers);

            return request;
        }

        /// <summary>
        /// Builds a Get Request for the method
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameterString">The parameter string.</param>
        /// <param name="controllerOverride">The controller override.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>
        /// RestRequest.
        /// </returns>
        protected RestRequest BuildGetRequest(string methodName, string parameterString, string controllerOverride = null, Dictionary<string, string> headers = null)
        {
            var request = new RestRequest(CalculateUrlForMethod(methodName, parameterString, controllerOverride), Method.Get);

            ApplyHeaders(request, headers);

            return request;
        }

        /// <summary>
        /// Applies any custom headers.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="customHeaders">Optional custom headers.</param>
        public void ApplyHeaders(RestRequest request, Dictionary<string, string> customHeaders = null)
        {
            if (customHeaders != null && customHeaders.Count > 0)
            {
                request.AddHeaders(customHeaders);
            }
        }

        /// <summary>
        /// Executes the an anonymous get call
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterString">The parameter string.</param>
        /// <param name="controllerOverride">The controller override.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.NoServerResponseException"></exception>
        /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.UnauthorisedException"></exception>
        /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.ServerResponseFailureException"></exception>
        /// <exception cref="System.Exception">Unexpected response</exception>
        public async Task<T> ExecuteGetAsync<T>(string actionName, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers);

                var result = await RestClient.ExecuteAsync<T>(request, cancellationToken: cancellationToken);

                if (!result.IsSuccessful)
                {
                    if (result.StatusCode == 0)
                    {
                        throw new NoServerResponseException(result.ErrorMessage, result.ErrorException);
                    }
                    else if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorisedException();
                    }
                    else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new ServerResponseFailureException(result.StatusCode, result.ErrorMessage, result.ErrorException);
                    }
                }

                return result.Data;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Executes the an anonymous get call
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="controllerOverride">The controller override.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unexpected response</exception>
        public async Task<T> ExecutePostAsync<T, T2>(string actionName, T2 payload, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
        {
            var request = BuildPostRequest(actionName, controllerOverride, headers);

            request.AddBody(payload);

            var result = await RestClient.ExecuteAsync<T>(request, cancellationToken: cancellationToken);

            if (!result.IsSuccessful)
            {
                if (result.StatusCode == 0)
                {
                    throw new NoServerResponseException(result.ErrorMessage, result.ErrorException);
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorisedException();
                }
                else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new ServerResponseFailureException(result.StatusCode, result.ErrorMessage, result.ErrorException);
                }
            }

            return result.Data;
        }

        /// <summary>
        /// Execute a Request asynchronously
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="request">Request</param>
        /// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
        /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.NoServerResponseException"></exception>
        /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.ServerResponseFailureException"></exception>
        /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.DataResponseFailureException"></exception>
        public async Task<T> ExecuteRequestAsync<T>(RestRequest request) where T : ResponseBase
        {
            var result = await RestClient.ExecuteAsync<T>(request);

            if (!result.IsSuccessful)
            {
                if (result.StatusCode == 0)
                {
                    throw new NoServerResponseException(result.ErrorMessage, result.ErrorException);
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorisedException();
                }
                else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new ServerResponseFailureException(result.StatusCode, result.ErrorMessage, result.ErrorException);
                }
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
        /// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
        /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.NoServerResponseException"></exception>
        /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.ServerResponseFailureException"></exception>
        /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.DataResponseFailureException"></exception>
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
                {
                    throw new NoServerResponseException(result.ErrorMessage, result.ErrorException);
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorisedException();
                }
                else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new ServerResponseFailureException(result.StatusCode, result.ErrorMessage, result.ErrorException);
                }
            }

            if (!result.Data.Success)
            {
                throw new DataResponseFailureException(result.Data.Message);
            }

            return result.Data;
        }

        #endregion

    }

}
