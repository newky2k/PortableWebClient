using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DSoft.Portable.WebClient.Rest.Enums;
using DSoft.Portable.WebClient.Rest.Exceptions;
using DSoft.Portable.WebClient.Rest.Responses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Base Service Client  class for consuming services provided by ASP.NET ApiControllers
/// </summary>
public abstract class RestServiceClientBase : IRestServiceClient, IDisposable
{
    #region Fields
    private readonly RestApiClientOptions _options;
    private readonly PortableRestHttpClient _httpClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IJwtTokenManger _tokenManager;
    private ICookieManager _cookieManager;
    #endregion

    #region Properties

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
            if (_options.HttpMessageHandler != null)
                return new HttpClient(_options.HttpMessageHandler);

            if (_cookieManager is CookieContainer cookies)
            {
                var handler = new HttpClientHandler { CookieContainer = cookies };
                return new HttpClient(handler);
            }

            return _httpClient?.HttpClient ?? new HttpClient();
        }
    }

    /// <summary>
    /// Custom Cookie Manager
    /// </summary>
    protected ICookieManager CookieManager
    {
        get
        {
            if (_cookieManager == null)
            {
                if (_serviceScopeFactory is not null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();

                    var cookieManager = scope.ServiceProvider.GetService<ICookieManager>();

                    _cookieManager = cookieManager;
                }
            }

            return _cookieManager;
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
            if (_tokenManager == null)
            {
                if (_serviceScopeFactory is not null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();

                    var tokenManager = scope.ServiceProvider.GetService<IJwtTokenManger>();

                    _tokenManager = tokenManager;
                }
            }

            return _tokenManager;

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

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RestServiceClientBase"/> class with the specified configuration options.
    /// </summary>
    /// <param name="options">The configuration options for the REST API client. If null, a new instance of <see cref="RestApiClientOptions"/> with default settings will be used.</param>
    /// <param name="httpClient"></param>
    /// <param name="serviceScopeFactory">Scope Factory</param>
    public RestServiceClientBase(IOptions<RestApiClientOptions> options, PortableRestHttpClient httpClient, IServiceScopeFactory serviceScopeFactory) : this(options?.Value) {
        _httpClient = httpClient;
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>
    /// Initializes a new instance of the RestServiceClientBase class using the specified REST API client options.
    /// </summary>
    /// <remarks>If the options parameter is null, a new instance of RestApiClientOptions is created and used
    /// as the default configuration.</remarks>
    /// <param name="options">The options that configure the behavior of the REST API client. If null, default options are used.</param>

    public RestServiceClientBase(RestApiClientOptions options)
    {
        _options = options ?? new RestApiClientOptions();
    }

    /// <summary>
    /// Initializes a new instance of the RestServiceClientBase class using the specified REST API client options.
    /// </summary>
    /// <remarks>If the options parameter is null, a new instance of RestApiClientOptions is created and used
    /// as the default configuration.</remarks>
    /// <param name="options">The options that configure the behavior of the REST API client. If null, default options are used.</param>
    /// <param name="tokenManager">Token Manager instance</param>
    public RestServiceClientBase(RestApiClientOptions options, IJwtTokenManger tokenManager)
    {
        _tokenManager ??= tokenManager;
        _options = options ?? new RestApiClientOptions();
    }

    /// <summary>
    /// Initializes a new instance of the RestServiceClientBase class using the specified REST API client options.
    /// </summary>
    /// <remarks>If the options parameter is null, a new instance of RestApiClientOptions is created and used
    /// as the default configuration.</remarks>
    /// <param name="options">The options that configure the behavior of the REST API client. If null, default options are used.</param>
    /// <param name="cookieManager">Cookie Manager</param>
    public RestServiceClientBase(RestApiClientOptions options, ICookieManager cookieManager )
    {
        _cookieManager ??= _cookieManager;
        _options = options ?? new RestApiClientOptions();
    }

    /// <summary>
    /// Initializes a new instance of the RestServiceClientBase class using the specified REST API client options.
    /// </summary>
    /// <remarks>If the options parameter is null, a new instance of RestApiClientOptions is created and used
    /// as the default configuration.</remarks>
    /// <param name="options">The options that configure the behavior of the REST API client. If null, default options are used.</param>
    /// <param name="tokenManager">Token Manager instance</param>
    /// <param name="cookieManager">Cookie Manager</param>
    public RestServiceClientBase(RestApiClientOptions options, IJwtTokenManger tokenManager, ICookieManager cookieManager)
    {
        _tokenManager ??= tokenManager;
        _cookieManager ??= _cookieManager;
        _options = options ?? new RestApiClientOptions();
    }

    #endregion

    #region Methods

    #region Client Builder

    private HttpClient GetHttpClient()
    {
        if (_httpClient != null)
            return _httpClient.HttpClient;

        HttpClient client;

        if (_options.HttpMessageHandler != null)
        {
            client = new HttpClient(_options.HttpMessageHandler);
        }
        else if (_cookieManager is CookieContainer cookies)
        {
            var handler = new HttpClientHandler { CookieContainer = cookies };
            client = new HttpClient(handler);
        }
        else
        {
            client = new HttpClient();
        }

        client.Timeout = _options.TimeOut;
        return client;
    }

    private void ResolveRequestUri(HttpRequestMessage request, string uniqueId, Uri baseAddressOverride = null)
    {
        if (request.RequestUri?.IsAbsoluteUri == true)
            return;

        var baseUri = baseAddressOverride ?? _options.UrlBuilder?.Invoke(uniqueId);

        if (baseUri != null && request.RequestUri != null)
            request.RequestUri = new Uri(baseUri, request.RequestUri);
    }

    private async Task<HttpResponseMessage> SendAndHandleAsync(HttpRequestMessage request, string uniqueId, Uri baseAddressOverride, RequestAuthenticationType? authenticationOverride, CancellationToken cancellationToken)
    {
        ResolveRequestUri(request, uniqueId, baseAddressOverride);

        await PreflightAsync(request, uniqueId, authenticationOverride);

        HttpResponseMessage response;

        try
        {
            response = await GetHttpClient().SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new NoServerResponseException(ex.Message, ex);
        }

        await HandleResponseAsync(response, uniqueId, authenticationOverride);

        return response;
    }

    private async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content))
            return default;

        var contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;

        if (!contentType.Contains("json", StringComparison.OrdinalIgnoreCase))
        {
            // Plain-text or other non-JSON response: return raw string for string types,
            // otherwise fall through to JSON (will throw if content isn't valid JSON).
            if (typeof(T) == typeof(string))
                return (T)(object)content;
        }

        return JsonSerializer.Deserialize<T>(content, _options.JsonSerializerOptions);
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
    public void ApplyHeaders(HttpRequestMessage request, Dictionary<string, string> customHeaders = null)
    {
        ApplyHeaderDictionary(request, _options.DefaultHeaders);
        ApplyHeaderDictionary(request, customHeaders);
    }

    private static void ApplyHeaderDictionary(HttpRequestMessage request, Dictionary<string, string> headers)
    {
        if (headers == null || headers.Count == 0)
            return;

        foreach (var kvp in headers)
        {
            request.Headers.Remove(kvp.Key);
            request.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
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
    /// HttpRequestMessage.
    /// </returns>
    protected HttpRequestMessage BuildPostRequest(string methodName, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, CalculateUrlForMethod(methodName, null, controllerOverride, serviceOverride));

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
    /// HttpRequestMessage.
    /// </returns>
    protected HttpRequestMessage BuildGetRequest(string methodName, string parameterString, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, CalculateUrlForMethod(methodName, parameterString, controllerOverride, serviceOverride));

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
    /// HttpRequestMessage.
    /// </returns>
    protected HttpRequestMessage BuildDeleteRequest(string methodName, string parameterString, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, CalculateUrlForMethod(methodName, parameterString, controllerOverride, serviceOverride));

        ApplyHeaders(request, headers);

        return request;
    }

    /// <summary>
    /// Builds basic Get Request with the specified Url
    /// </summary>
    /// <param name="url">The Url.</param>
    /// <param name="headers">The headers.</param>
    /// <returns>
    /// HttpRequestMessage.
    /// </returns>
    protected HttpRequestMessage BuildGetRequest(string url, Dictionary<string, string> headers = null) => BuildGetRequest(new Uri(url), headers);

    /// <summary>
    /// Builds basic Get Request with the specified Url
    /// </summary>
    /// <param name="url">The Url.</param>
    /// <param name="headers">The headers.</param>
    /// <returns>
    /// HttpRequestMessage.
    /// </returns>
    protected HttpRequestMessage BuildGetRequest(Uri url, Dictionary<string, string> headers = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        ApplyHeaders(request, headers);

        return request;
    }

    #endregion

    #region Executions

    #region Standard

    /// <summary>
    /// Execute an HTTP Get request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="actionName">Name of the action.</param>
    /// <param name="authenticationOverride">Type of authentication</param>
    /// <param name="parameterString">The parameter string.</param>
    /// <param name="controllerOverride">The controller override.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="UnauthorisedException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="System.Exception">Unexpected response</exception>
    public Task<T> ExecuteGetAsync<T>(string actionName, string controllerOverride = null, RequestAuthenticationType? authenticationOverride = null, string parameterString = null,
        Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers);

        return ExecuteAsync<T>(request, authenticationOverride, cancellationToken: cancellationToken);
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
    /// <param name="authenticationOverride">Specifies the authentication type to use for the request. Defaults to Anonymous if not provided.</param>
    /// <param name="parameterString">An optional string containing query parameters to include in the request. If null, no additional parameters are
    /// added.</param>
    /// <param name="headers">An optional dictionary of headers to include in the request. If null, no custom headers are sent.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete. Allows the operation to be cancelled.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the response deserialized as the
    /// specified type.</returns>
    public Task<T> ExecuteGetAsync<T>(string actionName, string controllerOverride, string serviceOverride, RequestAuthenticationType? authenticationOverride = null, string parameterString = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers, serviceOverride);

        return ExecuteAsync<T>(request, authenticationOverride, cancellationToken: cancellationToken);

    }

    /// <summary>
    /// Execute an HTTP Post request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <param name="actionName">Name of the action.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="authenticationOverride">Type of authentication</param>
    /// <param name="controllerOverride">The controller override.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.Exception">Unexpected response</exception>
    public Task<T> ExecutePostAsync<T, T2>(string actionName, T2 payload, RequestAuthenticationType? authenticationOverride = null, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildPostRequest(actionName, controllerOverride, headers);

        request.Content = new StringContent(JsonSerializer.Serialize(payload, _options.JsonSerializerOptions), Encoding.UTF8, "application/json");

        return ExecuteAsync<T>(request, authenticationOverride, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Execute a Post request asynchronously
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="actionName">Controller action name</param>
    /// <param name="packetBuilder">Function to buile request body object</param>
    /// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
    /// <param name="authenticationOverride">Type of authentication</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="DataResponseFailureException"></exception>
    public async Task<T> ExecutePostAsync<T>(string actionName, Func<object> packetBuilder, RequestAuthenticationType? authenticationOverride = null, CancellationToken cancellationToken = default) where T : ResponseBase
    {
        var request = BuildPostRequest(actionName);

        var body = packetBuilder?.Invoke();

        if (body != null)
            request.Content = new StringContent(JsonSerializer.Serialize(body, _options.JsonSerializerOptions), Encoding.UTF8, "application/json");

        return await ExecuteAsync<T>(request, authenticationOverride);

    }

    /// <summary>
    /// Executes an asynchronous HTTP DELETE request to the specified server action and returns the response of the
    /// specified type.
    /// </summary>
    /// <typeparam name="T">The type of the response expected from the DELETE request.</typeparam>
    /// <param name="actionName">The name of the server action to invoke for the DELETE request. Cannot be null or empty.</param>
    /// <param name="authenticationOverride">The authentication type to use for the request. Defaults to <see cref="RequestAuthenticationType.Anonymous"/> if
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
    public Task<T> ExecuteDeleteAsync<T>(string actionName, RequestAuthenticationType? authenticationOverride = null, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildDeleteRequest(actionName, parameterString, controllerOverride, headers);

        return ExecuteAsync<T>(request, authenticationOverride, cancellationToken);

    }

    #endregion

    #region Address overrideable

    /// <summary>
    /// Execute an HTTP Get request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="baseAddress"></param>
    /// <param name="actionName">Name of the action.</param>
    /// <param name="authenticationOverride">Type of authentication</param>
    /// <param name="parameterString">The parameter string.</param>
    /// <param name="controllerOverride">The controller override.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="UnauthorisedException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="System.Exception">Unexpected response</exception>
    public Task<T> ExecuteGetAsync<T>(Uri baseAddress, string actionName, string controllerOverride = null, RequestAuthenticationType? authenticationOverride = null, string parameterString = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers);

        return ExecuteAsync<T>(baseAddress, request, authenticationOverride, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Executes an asynchronous HTTP GET request to the specified action and returns the result as the specified type.
    /// </summary>
    /// <remarks>Use this method to perform a GET request when you need to specify custom authentication,
    /// headers, or override the default service or controller. The method supports cancellation via the provided
    /// token.</remarks>
    /// <typeparam name="T">The type of the result expected from the GET request.</typeparam>
    /// <param name="baseAddress"></param>
    /// <param name="actionName">The name of the action to invoke on the server. Cannot be null or empty.</param>
    /// <param name="controllerOverride">An optional controller name that overrides the default controller. If null, the default controller is used.</param>
    /// <param name="serviceOverride">An optional service URL that overrides the default service endpoint. If null, the default endpoint is used.</param>
    /// <param name="authenticationOverride">Specifies the authentication type to use for the request. Defaults to Anonymous if not provided.</param>
    /// <param name="parameterString">An optional string containing query parameters to include in the request. If null, no additional parameters are
    /// added.</param>
    /// <param name="headers">An optional dictionary of headers to include in the request. If null, no custom headers are sent.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete. Allows the operation to be cancelled.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the response deserialized as the
    /// specified type.</returns>
    public Task<T> ExecuteGetAsync<T>(Uri baseAddress, string actionName, string controllerOverride, string serviceOverride, RequestAuthenticationType? authenticationOverride = null, string parameterString = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers, serviceOverride);

        return ExecuteAsync<T>(baseAddress, request, authenticationOverride, cancellationToken: cancellationToken);

    }

    /// <summary>
    /// Execute an HTTP Post request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <param name="baseAddress"></param>
    /// <param name="actionName">Name of the action.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="authenticationOverride">Type of authentication</param>
    /// <param name="controllerOverride">The controller override.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.Exception">Unexpected response</exception>
    public Task<T> ExecutePostAsync<T, T2>(Uri baseAddress, string actionName, T2 payload, RequestAuthenticationType? authenticationOverride = null, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildPostRequest(actionName, controllerOverride, headers);

        request.Content = new StringContent(JsonSerializer.Serialize(payload, _options.JsonSerializerOptions), Encoding.UTF8, "application/json");

        return ExecuteAsync<T>(baseAddress, request, authenticationOverride, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Execute a Post request asynchronously
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="baseAddress"></param>
    /// <param name="actionName">Controller action name</param>
    /// <param name="packetBuilder">Function to buile request body object</param>
    /// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
    /// <param name="authenticationOverride">Type of authentication</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="DataResponseFailureException"></exception>
    public async Task<T> ExecutePostAsync<T>(Uri baseAddress, string actionName, Func<object> packetBuilder, RequestAuthenticationType? authenticationOverride = null, CancellationToken cancellationToken = default) where T : ResponseBase
    {
        var request = BuildPostRequest(actionName);

        var body = packetBuilder?.Invoke();

        if (body != null)
            request.Content = new StringContent(JsonSerializer.Serialize(body, _options.JsonSerializerOptions), Encoding.UTF8, "application/json");

        return await ExecuteAsync<T>(baseAddress, request, authenticationOverride);

    }

    /// <summary>
    /// Executes an asynchronous HTTP DELETE request to the specified server action and returns the response of the
    /// specified type.
    /// </summary>
    /// <typeparam name="T">The type of the response expected from the DELETE request.</typeparam>
    /// <param name="baseAddress"></param>
    /// <param name="actionName">The name of the server action to invoke for the DELETE request. Cannot be null or empty.</param>
    /// <param name="authenticationOverride">The authentication type to use for the request. Defaults to <see cref="RequestAuthenticationType.Anonymous"/> if
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
    public Task<T> ExecuteDeleteAsync<T>(Uri baseAddress, string actionName, RequestAuthenticationType? authenticationOverride = null, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var request = BuildDeleteRequest(actionName, parameterString, controllerOverride, headers);

        return ExecuteAsync<T>(baseAddress, request, authenticationOverride, cancellationToken);

    }

    #endregion

    #region Core

    /// <summary>
    /// Execute a Request asynchronously
    /// </summary>
    /// <typeparam name="T">Response type</typeparam>
    /// <param name="request">Request</param>
    /// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
    /// <param name="authenticationOverride">Type of authentication</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="DataResponseFailureException"></exception>
    public async Task<T> ExecuteRequestWithBaseResonseAsync<T>(HttpRequestMessage request, RequestAuthenticationType? authenticationOverride = null, CancellationToken cancellationToken = default) where T : ResponseBase
    {
        var result = await ExecuteAsync<T>(request, authenticationOverride, cancellationToken);

        if (result?.Success == false)
            throw new DataResponseFailureException(result.Message);

        return result;
    }

    /// <summary>
    /// Executes the specified REST request asynchronously and returns the response data of the specified type.
    /// </summary>
    /// <remarks>Performs a preflight check based on the specified authentication type before executing the
    /// request. Handles the response to ensure proper processing according to the authentication method.</remarks>
    /// <typeparam name="T">The type of the data expected in the response from the REST request.</typeparam>
    /// <param name="request">The REST request to execute. Must contain all necessary information for the API call.</param>
    /// <param name="authenticationOverride">The authentication type to use for the request. Defaults to <see cref="RequestAuthenticationType.Anonymous"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The response data of type <typeparamref name="T"/> from the executed REST request.</returns>
    public async Task<T> ExecuteAsync<T>(HttpRequestMessage request, RequestAuthenticationType? authenticationOverride = null, CancellationToken cancellationToken = default)
    {
        var uniqueId = await GetUniqueIdAsync();

        using var response = await SendAndHandleAsync(request, uniqueId, null, authenticationOverride, cancellationToken);

        return await DeserializeAsync<T>(response);
    }

    /// <summary>
    /// Executes the specified REST request asynchronously and returns the response data of the specified type.
    /// </summary>
    /// <remarks>Performs a preflight check based on the specified authentication type before executing the
    /// request. Handles the response to ensure proper processing according to the authentication method.</remarks>
    /// <typeparam name="T">The type of the data expected in the response from the REST request.</typeparam>
    /// <param name="baseAddress"></param>
    /// <param name="request">The REST request to execute. Must contain all necessary information for the API call.</param>
    /// <param name="authenticationOverride">The authentication type to use for the request. Defaults to <see cref="RequestAuthenticationType.Anonymous"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The response data of type <typeparamref name="T"/> from the executed REST request.</returns>
    public async Task<T> ExecuteAsync<T>(Uri baseAddress, HttpRequestMessage request, RequestAuthenticationType? authenticationOverride = null, CancellationToken cancellationToken = default)
    {
        var uniqueId = await GetUniqueIdAsync();

        using var response = await SendAndHandleAsync(request, uniqueId, baseAddress, authenticationOverride, cancellationToken);

        return await DeserializeAsync<T>(response);
    }

    /// <summary>
    /// Executes an asynchronous HTTP GET request to retrieve binary data from the specified action endpoint.
    /// </summary>
    /// <remarks>The file name is extracted from the 'content-disposition' header if present in the response.
    /// Use this method to download files or other binary content from a REST endpoint.</remarks>
    /// <param name="actionName">The name of the action to invoke, which determines the target endpoint for the GET request.</param>
    /// <param name="authenticationOverride">Specifies the authentication type to use for the request. Defaults to <see
    /// cref="RequestAuthenticationType.Anonymous"/> if not provided.</param>
    /// <param name="parameterString">An optional string containing additional query parameters to include in the request URL.</param>
    /// <param name="controllerOverride">An optional controller name to override the default controller used for the request.</param>
    /// <param name="headers">An optional dictionary of HTTP headers to include in the request, allowing customization of request behavior.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="DownloadResult"/> containing the binary data and the associated file name, if available.</returns>
    protected async Task<DownloadResult> ExecuteGetBinaryAsync(string actionName, RequestAuthenticationType? authenticationOverride = null, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
    {
        var uniqueId = await GetUniqueIdAsync();

        var request = BuildGetRequest(actionName, parameterString, controllerOverride, headers);

        using var response = await SendAndHandleAsync(request, uniqueId, null, authenticationOverride, cancellationToken);

        var filename = response.Content.Headers.ContentDisposition?.FileName ?? string.Empty;
        var data = await response.Content.ReadAsByteArrayAsync();

        return new DownloadResult() { Data = data, FileName = filename };
    }

    #endregion

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
    /// <param name="request"></param>
    /// <param name="uniqueId"></param>
    /// <param name="authenticationOverride">Specifies the authentication method to use for the request. The value determines how preflight checks are
    /// performed and which authentication mechanisms are validated.</param>
    public async Task PreflightAsync(HttpRequestMessage request, string uniqueId, RequestAuthenticationType? authenticationOverride = null)
    {
        RequestAuthenticationType authentication = authenticationOverride ?? _options.AuthenticationType;

        switch (authentication)
        {
            case RequestAuthenticationType.Cookie:
            {
                try
                {
                    if (CookieManager != null && !CookieManager.HasValidUserCookies)
                    {
                        await CookieManager.LoadCookiesAsync(uniqueId);
                    }

                    if (!CookieManager.HasValidUserCookies)
                    {
                        throw new InvalidCookiesException();
                    }
                }
                catch (Exception)
                {
                    await HandleAuthFailureAsync(uniqueId, authenticationOverride);
                }
            }
            break;
            case RequestAuthenticationType.Token:
            {
                if (TokenManager is not null)
                {
                    var accessToken = await TokenManager.LoadAccessTokenAsync(uniqueId);

                    if (string.IsNullOrWhiteSpace(accessToken))
                    {
                        return;
                    }

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
            }
            break;
        }
    }

    /// <summary>
    /// Handles the response from the server
    /// </summary>
    /// <param name="response">The response.</param>
    /// <param name="uniqueId"></param>
    /// <param name="authenticationOverride">Type of authentication</param>
    /// <exception cref="NoServerResponseException"></exception>
    /// <exception cref="ServerResponseFailureException"></exception>
    /// <exception cref="UnauthorisedException"></exception>
    public async Task HandleResponseAsync(HttpResponseMessage response, string uniqueId, RequestAuthenticationType? authenticationOverride = null)
    {
        RequestAuthenticationType authentication = authenticationOverride ?? _options.AuthenticationType;

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await HandleAuthFailureAsync(uniqueId, authenticationOverride);
            }
            else
            {
                throw new ServerResponseFailureException(response.StatusCode, response.ReasonPhrase, null);
            }
        }

        switch (authentication)
        {
            case RequestAuthenticationType.Cookie:
            {
                try
                {
                    await CookieManager?.ValidateAndSaveAsync(uniqueId);
                }
                catch (Exception)
                {
                    await HandleAuthFailureAsync(uniqueId, authenticationOverride);
                }
            }
            break;
        }

    }

    /// <summary>
    /// Handles the authentication failure for the unique connection id or base address
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <param name="authenticationOverride"></param>
    public async Task HandleAuthFailureAsync(string uniqueId, RequestAuthenticationType? authenticationOverride = null)
    {
        RequestAuthenticationType authentication = authenticationOverride ?? _options.AuthenticationType;

        switch (authentication)
        {
            case RequestAuthenticationType.Cookie:
            {
                await CookieManager?.DeleteCookiesAsync(uniqueId);
            }
            break;
            case RequestAuthenticationType.Token:
            {
                await TokenManager?.HandleAuthFailureAsync(uniqueId);
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

    public virtual Task<string> GetUniqueIdAsync()
    {
        return Task.FromResult(string.Empty);
    }


    #endregion

    #endregion

}
