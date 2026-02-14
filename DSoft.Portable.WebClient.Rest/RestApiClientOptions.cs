using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Options for the rest client
/// </summary>
public class RestApiClientOptions
{

    /// <summary>
    /// Gets the time out. Default(30 seconds)
    /// </summary>
    /// <value>The time out.</value>
    public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the optional cookie container.
    /// </summary>
    /// <value>
    /// The cookie container.
    /// </value>
    public CookieContainer CookieContainer { get; set; }

    /// <summary>
    /// Gets or sets the json serializer options for RestClient to override the defaults
    /// </summary>
    /// <value>
    /// The json serializer options.
    /// </value>
    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    /// <summary>
    /// Gets or sets the an optional HTTP message handler. (Use for Unit Testing)
    /// </summary>
    /// <value>The HTTP message handler.</value>
    public HttpMessageHandler HttpMessageHandler { get; set; }
}
