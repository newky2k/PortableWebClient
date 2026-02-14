using DSoft.Portable.WebClient.Core.Enum;
using DSoft.Portable.WebClient.Core.Exceptions;
using RestSharp;
using RestSharp.Serializers.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Base Service Client  class for consuming services provided by ASP.NET ApiControllers
/// </summary>
public abstract class RestServiceClientBase : IRestServiceClient, IDisposable
{
    #region Fields
    private Uri _baseAddress;
    private RestApiClientOptions _options;
    private string _connectionId;
    #endregion

    #region Properties

    /// <summary>
    /// Gets the rest client.
    /// </summary>
    /// <value>The rest client.</value>
    protected virtual IRestClient RestClient
    {
        get
        {

            var options = new RestClientOptions()
            {
                BaseUrl = _baseAddress,
                Timeout = _options.TimeOut,
            };

            HttpClient client = null;

            if (_options.HttpMessageHandler != null)
            {
                client = new HttpClient(_options.HttpMessageHandler);
            }
            else if (_options.CookieContainer != null)
            {
                var handler = new HttpClientHandler { CookieContainer = _options.CookieContainer };
                client = new HttpClient(handler);

                options.CookieContainer = _options.CookieContainer;
            }

            return BuildRestClient(options, client);

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
    /// Gets the options provided to the client
    /// </summary>
    /// <value>The options.</value>
    public RestApiClientOptions Options => _options;

    /// <summary>
    /// Gets the client version no.
    /// </summary>
    /// <value>The client version no.</value>
    public abstract string ClientVersionNo { get; }

    /// <summary>
    /// Gets the default headers.
    /// </summary>
    /// <value>The default headers.</value>
    protected virtual Dictionary<string, string> DefaultHeaders => null;

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

    private string UniqueId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_connectionId))
            {
                return _baseAddress.ToString();
            }

            return _connectionId;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Configure the service
    /// </summary>
    /// <param name="baseAddress">base address for requests</param>
    /// <param name="options">Congiuration options</param>
    /// <param name="uniqueId">Unique id or connection key</param>
    public void Configure(Uri baseAddress, RestApiClientOptions options, string uniqueId = null)
    {
        _baseAddress = baseAddress;
        _options = options;
        _connectionId = uniqueId;
    }

    #region Client Builder

    private RestClient BuildRestClient(RestClientOptions options, HttpClient httpClient = null)
    {
        if (_options.JsonSerializerOptions is not null)
        {
            if (httpClient is null)
            {
                return new RestClient(options,
                   configureSerialization: s => s.UseSystemTextJson(_options.JsonSerializerOptions));
            }
            else
            {
                return new RestClient(httpClient,
                   options,
                   configureSerialization: s => s.UseSystemTextJson(_options.JsonSerializerOptions));
            }

        }
        else
        {
            if (httpClient is null)
            {
                return new RestClient(options);
            }
            else
            {
                return new RestClient(httpClient, options);
            }
        }
    }

    #endregion

    #region Request Calculations

    /// <summary>
    /// Calculates the URL for method.
    /// </summary>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="parameterString">The parameter string.</param>
    /// <param name="controllerOverride">The controller override.</param>
    /// <param name="serviceOverride">override the service component</param>
    /// <returns></returns>
    public string CalculateUrlForMethod(string methodName, string parameterString = null, string controllerOverride = null, string serviceOverride = null)
    {
        var apiPrefixBase = ApiPrefix;

        if (!string.IsNullOrEmpty(apiPrefixBase) && !apiPrefixBase.EndsWith(@"/"))
        {
            apiPrefixBase = $"{apiPrefixBase}/";
        }

        var baseEndpointService = apiPrefixBase;

        var serviceComponent = ServiceName;

        if (!string.IsNullOrWhiteSpace(serviceOverride))
        {
            serviceComponent = serviceOverride;
        }

        if (!string.IsNullOrWhiteSpace(serviceComponent))
        {
            baseEndpointService = $"{apiPrefixBase}/{serviceComponent}/";
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
    /// Apply any custom headers.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="customHeaders">Optional custom headers.</param>
    public void ApplyHeaders(RestRequest request, Dictionary<string, string> customHeaders = null)
    {
        if (DefaultHeaders != null && DefaultHeaders.Count > 0)
        {
            request.AddHeaders(DefaultHeaders);
        }

        if (customHeaders != null && customHeaders.Count > 0)
        {
            request.AddHeaders(customHeaders);
        }
    }

    /// <summary>
    /// Builds a Post Request for the method
    /// </summary>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="controllerOverride">The controller override.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="serviceOverride">override the service component</param>
    /// <returns>
    /// RestRequest.
    /// </returns>
    protected RestRequest BuildPostRequest(string methodName, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new RestRequest(CalculateUrlForMethod(methodName, null, controllerOverride, serviceOverride), Method.Post);

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
    /// <param name="serviceOverride">override the service component</param>
    /// <returns>
    /// RestRequest.
    /// </returns>
    protected RestRequest BuildGetRequest(string methodName, string parameterString, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new RestRequest(CalculateUrlForMethod(methodName, parameterString, controllerOverride, serviceOverride), Method.Get);

        ApplyHeaders(request, headers);

        return request;
    }

    /// <summary>
    /// Builds a Delete Request for the method
    /// </summary>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="parameterString">The parameter string.</param>
    /// <param name="controllerOverride">The controller override.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="serviceOverride">override the service component</param>
    /// <returns>
    /// RestRequest.
    /// </returns>
    protected RestRequest BuildDeleteRequest(string methodName, string parameterString, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new RestRequest(CalculateUrlForMethod(methodName, parameterString, controllerOverride, serviceOverride), Method.Delete);

        ApplyHeaders(request, headers);

        return request;
    }

    /// <summary>
    /// Builds basic Get Request with the specified Url
    /// </summary>
    /// <param name="url">The Url.</param>
    /// <param name="headers">The headers.</param>
    /// <returns>
    /// RestRequest.
    /// </returns>
    protected RestRequest BuildGetRequest(string url, Dictionary<string, string> headers = null) => BuildGetRequest(new Uri(url), headers);

    /// <summary>
    /// Builds basic Get Request with the specified Url
    /// </summary>
    /// <param name="url">The Url.</param>
    /// <param name="headers">The headers.</param>
    /// <returns>
    /// RestRequest.
    /// </returns>
    protected RestRequest BuildGetRequest(Uri url, Dictionary<string, string> headers = null)
    {
        var request = new RestRequest(url, Method.Get);

        ApplyHeaders(request, headers);

        return request;
    }

    #endregion

    #region Executions

    /// <summary>
    /// Execute an HTTP Get request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="actionName">Name of the action.</param>
    /// <param name="authentication">Type of authentication</param>
    /// <param name="parameterString">The parameter string.</param>
    /// <param name="controllerOverride">The controller override.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="UnauthorisedException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="System.Exception">Unexpected response</exception>
    public Task<T> ExecuteGetAsync<T>(string actionName, RequestAuthenticationType authentication = RequestAuthenticationType.None, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers);

        return ExecuteAsync<T>(request, authentication, cancellationToken: cancellationToken);

    }

    /// <summary>
    /// Execute an HTTP Post request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <param name="actionName">Name of the action.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="authentication">Type of authentication</param>
    /// <param name="controllerOverride">The controller override.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.Exception">Unexpected response</exception>
    public Task<T> ExecutePostAsync<T, T2>(string actionName, T2 payload, RequestAuthenticationType authentication = RequestAuthenticationType.None, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildPostRequest(actionName, controllerOverride, headers);

        request.AddBody(payload);

        return ExecuteAsync<T>(request, authentication, cancellationToken: cancellationToken); 
    }

    /// <summary>
    /// Execute a Post request asynchronously
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="actionName">Controller action name</param>
    /// <param name="packetBuilder">Function to buile request body object</param>
    /// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
    /// <param name="authentication">Type of authentication</param>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="DataResponseFailureException"></exception>
    public async Task<T> ExecutePostAsync<T>(string actionName, Func<object> packetBuilder,  RequestAuthenticationType authentication = RequestAuthenticationType.None, CancellationToken cancellationToken = default) where T : ResponseBase
    {
        var request = BuildPostRequest(actionName);

        var body = packetBuilder?.Invoke();

        if (body != null)
            request.AddJsonBody(body);

        return await ExecuteRequestAsync<T>(request, authentication);

    }

    /// <summary>
    /// Execute a Request asynchronously
    /// </summary>
    /// <typeparam name="T">Response type</typeparam>
    /// <param name="request">Request</param>
    /// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
    /// <param name="authentication">Type of authentication</param>
    /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.NoServerResponseException"></exception>
    /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.ServerResponseFailureException"></exception>
    /// <exception cref="DSoft.Portable.WebClient.Core.Exceptions.DataResponseFailureException"></exception>
    public async Task<T> ExecuteRequestAsync<T>(RestRequest request, RequestAuthenticationType authentication = RequestAuthenticationType.None, CancellationToken cancellationToken = default) where T : ResponseBase
    {
        Preflight(authentication);

        var result = await RestClient.ExecuteAsync<T>(request, cancellationToken: cancellationToken);

        HandleResponse(result, authentication);

        if (result.Data.Success == false)
            throw new DataResponseFailureException(result.Data.Message);

        return result.Data;
    }

    private async Task<T> ExecuteAsync<T>(RestRequest request, RequestAuthenticationType authentication = RequestAuthenticationType.None, CancellationToken cancellationToken = default)
    {
        Preflight(authentication);

        var result = await RestClient.ExecuteAsync<T>(request, cancellationToken: cancellationToken);

        HandleResponse(result, authentication);

        return result.Data;
    }

    #endregion

    private void Preflight(RequestAuthenticationType authentication)
    {
        switch (authentication)
        {
            case RequestAuthenticationType.Cookie:
                {
                    try
                    {
                        if (CookieManager != null && !CookieManager.HasValidUserCookies)
                        {
                            CookieManager.LoadCookies(UniqueId);
                        }

                        if (!CookieManager.HasValidUserCookies)
                        {
                            throw new InvalidCookiesException();
                        }
                    }
                    catch (Exception ex)
                    {
                        HandleAuthFailure(UniqueId, authentication);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Handles the response from the server
    /// </summary>
    /// <param name="response">The response.</param>
    /// <param name="authentication">Type of authentication</param>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="UnauthorisedException"></exception>
    private void HandleResponse(RestResponse response, RequestAuthenticationType authentication)
    {
        if (!response.IsSuccessful)
        {
            if (response.StatusCode == 0)
            {
                throw new NoServerResponseException(response.ErrorMessage, response.ErrorException);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HandleAuthFailure(UniqueId, authentication);
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ServerResponseFailureException(response.StatusCode, response.ErrorMessage, response.ErrorException);
            }
            else
            {
                throw new Exception("Unexpected response from the server");

            }

        }

        switch (authentication)
        {
            case RequestAuthenticationType.Cookie:
                {
                    try
                    {
                        CookieManager?.ValidateAndSave(UniqueId);
                    }
                    catch (Exception ex)
                    {
                        HandleAuthFailure(UniqueId, authentication);
                    }
                }
                break;
        }

    }

    /// <summary>
    /// Handles the authentication failure for the unique connection id or base address
    /// </summary>
    /// <param name="connectionId">Unique connection id or base address</param>
    /// <param name="authentication"></param>
    private void HandleAuthFailure(string connectionId, RequestAuthenticationType authentication)
    {     
        switch (authentication)
        {
            case RequestAuthenticationType.Cookie:
                {
                    CookieManager?.DeleteCookies(connectionId);
                }
                break;
        }

        throw new UnauthorisedException();
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {

    }

    #endregion

}
