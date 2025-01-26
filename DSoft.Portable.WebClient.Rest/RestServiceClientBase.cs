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
		protected virtual IRestClient RestClient => new RestClient(_options.BaseUrl, options =>
		{
			options.Timeout = _options.TimeOut;

		});

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
		/// <value>The api prefix - default: api</value>
		protected virtual string ApiPrefix => "";

        /// <summary>
        /// Gets the API prefix inserted before the controller name E.g. api/controller/method
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
        public string CalculateUrlForMethod(string methodName, string parameterString, string controllerOverride = null)
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
        private void ApplyHeaders(RestRequest request, Dictionary<string, string> customHeaders = null)
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
        /// <param name="controllerOverride">The controller override.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unexpected response</exception>
        protected async Task<T> ExecuteGetAsync<T>(string actionName, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = BuildGetRequest(actionName, null, controllerOverride, headers);

                var result = await RestClient.ExecuteAsync<T>(request, cancellationToken: cancellationToken);

                if (!result.IsSuccessful)
                {
                    if (result.StatusCode == 0)
                    {
                        throw new NoServerResponseException(result.ErrorMessage, result.ErrorException);
                    }
                    else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new ServerResponseFailureException(result.StatusCode, result.ErrorMessage, result.ErrorException);
                    }
                    else
                    {
                        throw new Exception("Unexpected response");
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
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="controllerOverride">The controller override.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unexpected response</exception>
        protected async Task<T> ExecutePostAsync<T, T2>(Guid connectionId, string actionName, T2 payload, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
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
                else if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new ServerResponseFailureException(result.StatusCode, result.ErrorMessage, result.ErrorException);
                }
                else
                {
                    throw new Exception("Unexpected response");
                }
            }

            return result.Data;
        }

		#endregion

	}

}
