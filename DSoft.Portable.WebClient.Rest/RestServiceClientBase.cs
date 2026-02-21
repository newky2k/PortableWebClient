using DSoft.Portable.WebClient.Core.Exceptions;
using DSoft.Portable.WebClient.Rest.Enums;
using DSoft.Portable.WebClient.Rest.Responses;
using RestSharp;
using RestSharp.Serializers.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
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
    /// Gets an instance of <see cref="HttpClient"/> that is configured with a cookie container for authenticated HTTP
    /// requests.
    /// </summary>
    /// <remarks>This property creates a new <see cref="HttpClient"/> using the cookie container specified in
    /// the options. Use this client when making requests that require authentication via cookies. Each access returns a
    /// new instance; callers are responsible for managing the lifetime of the client as appropriate.</remarks>
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
    /// Gets the instance of the token manager responsible for handling JSON Web Tokens (JWT) for authentication and
    /// authorization operations.
    /// </summary>
    /// <remarks>Returns null if the token manager is not configured in the options. Ensure that the token
    /// manager is properly set in the options before accessing this property to avoid null reference issues.</remarks>
    protected IJwtTokenManger TokenManager
    {
        get
        {
            if (_options.TokenManger == null)
                return null;

            if (_options.TokenManger is IJwtTokenManger tm)
            {
                return tm;
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

    #region Setup

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
        if (_options.DefaultHeaders != null && _options.DefaultHeaders.Count > 0)
        {
            request.AddHeaders(_options.DefaultHeaders);
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
    public Task<T> ExecuteGetAsync<T>(string actionName, string controllerOverride = null, RequestAuthenticationType authentication = RequestAuthenticationType.Anonymous, string parameterString = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers);

        return ExecuteAsync<T>(request, authentication, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Executes an asynchronous HTTP GET request to the specified action and returns the result as the specified type.
    /// </summary>
    /// <remarks>Use this method to perform a GET request when you need to specify custom authentication,
    /// headers, or override the default service or controller. The method supports cancellation via the provided
    /// token.</remarks>
    /// <typeparam name="T">The type of the result expected from the GET request.</typeparam>
    /// <param name="actionName">The name of the action to invoke on the server. Cannot be null or empty.</param>
    /// <param name="controllerOverride">An optional controller name that overrides the default controller. If null, the default controller is used.</param>
    /// <param name="serviceOverride">An optional service URL that overrides the default service endpoint. If null, the default endpoint is used.</param>
    /// <param name="authentication">Specifies the authentication type to use for the request. Defaults to Anonymous if not provided.</param>
    /// <param name="parameterString">An optional string containing query parameters to include in the request. If null, no additional parameters are
    /// added.</param>
    /// <param name="headers">An optional dictionary of headers to include in the request. If null, no custom headers are sent.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete. Allows the operation to be cancelled.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the response deserialized as the
    /// specified type.</returns>
    public Task<T> ExecuteGetAsync<T>(string actionName, string controllerOverride, string serviceOverride, RequestAuthenticationType authentication = RequestAuthenticationType.Anonymous, string parameterString = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers, serviceOverride);

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
    public Task<T> ExecutePostAsync<T, T2>(string actionName, T2 payload, RequestAuthenticationType authentication = RequestAuthenticationType.Anonymous, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
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
    /// <param name="cancellationToken"></param>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="DataResponseFailureException"></exception>
    public async Task<T> ExecutePostAsync<T>(string actionName, Func<object> packetBuilder,  RequestAuthenticationType authentication = RequestAuthenticationType.Anonymous, CancellationToken cancellationToken = default) where T : ResponseBase
    {
        var request = BuildPostRequest(actionName);

        var body = packetBuilder?.Invoke();

        if (body != null)
            request.AddJsonBody(body);

        return await ExecuteAsync<T>(request, authentication);

    }

    /// <summary>
    /// Executes an asynchronous HTTP DELETE request to the specified server action and returns the response of the
    /// specified type.
    /// </summary>
    /// <typeparam name="T">The type of the response expected from the DELETE request.</typeparam>
    /// <param name="actionName">The name of the server action to invoke for the DELETE request. Cannot be null or empty.</param>
    /// <param name="authentication">The authentication type to use for the request. Defaults to <see cref="RequestAuthenticationType.Anonymous"/> if
    /// not specified.</param>
    /// <param name="parameterString">An optional string containing query parameters to include in the request. May be null if no parameters are
    /// required.</param>
    /// <param name="controllerOverride">An optional string to specify a different controller for handling the request. If null, the default controller
    /// is used.</param>
    /// <param name="headers">An optional dictionary of HTTP headers to include in the request. May be null if no additional headers are
    /// needed.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete. Allows the operation to be cancelled.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the response from the server as an
    /// instance of type T.</returns>
    public Task<T> ExecuteDeleteAsync<T>(string actionName, RequestAuthenticationType authentication = RequestAuthenticationType.Anonymous, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildDeleteRequest(actionName, parameterString, controllerOverride, headers);
     
        return ExecuteAsync<T>(request, authentication, cancellationToken) ;

    }
    
    /// <summary>
    /// Execute a Request asynchronously
    /// </summary>
    /// <typeparam name="T">Response type</typeparam>
    /// <param name="request">Request</param>
    /// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
    /// <param name="authentication">Type of authentication</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="DataResponseFailureException"></exception>
    public async Task<T> ExecuteRequestWithBaseResonseAsync<T>(RestRequest request, RequestAuthenticationType authentication = RequestAuthenticationType.Anonymous, CancellationToken cancellationToken = default) where T : ResponseBase
    {
        Preflight(authentication);

        var result = await RestClient.ExecuteAsync<T>(request, cancellationToken: cancellationToken);

        HandleResponse(result, authentication);

        if (result.Data.Success == false)
            throw new DataResponseFailureException(result.Data.Message);

        return result.Data;
    }

    /// <summary>
    /// Executes the specified REST request asynchronously and returns the response data of the specified type.
    /// </summary>
    /// <remarks>Performs a preflight check based on the specified authentication type before executing the
    /// request. Handles the response to ensure proper processing according to the authentication method.</remarks>
    /// <typeparam name="T">The type of the data expected in the response from the REST request.</typeparam>
    /// <param name="request">The REST request to execute. Must contain all necessary information for the API call.</param>
    /// <param name="authentication">The authentication type to use for the request. Defaults to <see cref="RequestAuthenticationType.Anonymous"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The response data of type <typeparamref name="T"/> from the executed REST request.</returns>
    public async Task<T> ExecuteAsync<T>(RestRequest request, RequestAuthenticationType authentication = RequestAuthenticationType.Anonymous, CancellationToken cancellationToken = default)
    {
        Preflight(authentication);

        var result = await RestClient.ExecuteAsync<T>(request, cancellationToken: cancellationToken);

        HandleResponse(result, authentication);

        return result.Data;
    }

    /// <summary>
    /// Executes an asynchronous HTTP GET request to retrieve binary data from the specified action endpoint.
    /// </summary>
    /// <remarks>The file name is extracted from the 'content-disposition' header if present in the response.
    /// Use this method to download files or other binary content from a REST endpoint.</remarks>
    /// <param name="actionName">The name of the action to invoke, which determines the target endpoint for the GET request.</param>
    /// <param name="authentication">Specifies the authentication type to use for the request. Defaults to <see
    /// cref="RequestAuthenticationType.Anonymous"/> if not provided.</param>
    /// <param name="parameterString">An optional string containing additional query parameters to include in the request URL.</param>
    /// <param name="controllerOverride">An optional controller name to override the default controller used for the request.</param>
    /// <param name="headers">An optional dictionary of HTTP headers to include in the request, allowing customization of request behavior.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="DownloadResult"/> containing the binary data and the associated file name, if available.</returns>
    protected async Task<DownloadResult> ExecuteGetBinaryAsync(string actionName, RequestAuthenticationType authentication = RequestAuthenticationType.Anonymous, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        Preflight(authentication);

        var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers);

        var result = await RestClient.ExecuteAsync(request, cancellationToken: cancellationToken);

        HandleResponse(result, authentication);

        var header_contentDisposition = result.ContentHeaders.FirstOrDefault(x => x.Name.Equals("content-disposition", StringComparison.OrdinalIgnoreCase));

        var filename = string.Empty;

        if (header_contentDisposition != null)
        {
            filename = new ContentDisposition(header_contentDisposition.Value).FileName;
        }

        return new DownloadResult() { Data = result.RawBytes, FileName = filename };

    }
    #endregion

    #region Pre And Post Flight methods

    /// <summary>
    /// Performs preflight authentication checks based on the specified authentication type to ensure the request is
    /// properly authorized before proceeding.
    /// </summary>
    /// <remarks>When the authentication type is set to Cookie, this method verifies the presence and validity
    /// of user cookies. If valid cookies are not found, it attempts to load them. If cookies remain invalid after
    /// loading, an InvalidCookiesException is thrown. This process is essential for confirming user authentication
    /// prior to executing further operations.</remarks>
    /// <param name="authentication">Specifies the authentication method to use for the request. The value determines how preflight checks are
    /// performed and which authentication mechanisms are validated.</param>
    public void Preflight(RequestAuthenticationType authentication)
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
    public void HandleResponse(RestResponse response, RequestAuthenticationType authentication)
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
    public void HandleAuthFailure(string connectionId, RequestAuthenticationType authentication)
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

    #endregion

}
