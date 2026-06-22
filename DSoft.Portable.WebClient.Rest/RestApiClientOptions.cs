using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using DSoft.Portable.WebClient.Rest.Enums;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Configuration passed to a <see cref="RestServiceClientBase"/>, controlling authentication,
/// timeouts, serialization, default headers, and how the base URL is resolved.
/// </summary>
public class RestApiClientOptions
{
    /// <summary>
    /// The authentication scheme to apply to requests. Defaults to <see cref="RequestAuthenticationType.Anonymous"/>.
    /// </summary>
    public RequestAuthenticationType AuthenticationType { get; set; } = RequestAuthenticationType.Anonymous;

    /// <summary>
    /// The per-request timeout. Defaults to 30 seconds.
    /// </summary>
    public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Custom JSON serializer options to override the client's defaults; leave null to use the defaults.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    /// <summary>
    /// An optional HTTP message handler to inject into the underlying client; primarily used to route
    /// requests through a test server (for example <c>WebApplicationFactory</c>).
    /// </summary>
    public HttpMessageHandler HttpMessageHandler { get; set; }

    /// <summary>
    /// Headers added to every request the client sends.
    /// </summary>
    public Dictionary<string, string> DefaultHeaders { get; set; } = [];

    /// <summary>
    /// An optional function that resolves the base URL from a connection key, allowing the address to be
    /// determined at runtime (for example when registered through dependency injection).
    /// </summary>
    public Func<string, Uri> UrlBuilder { get; set; }


}
