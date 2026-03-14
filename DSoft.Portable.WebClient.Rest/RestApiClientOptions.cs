using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using DSoft.Portable.WebClient.Rest.Enums;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Options for the rest client
/// </summary>
public class RestApiClientOptions
{
    /// <summary>
    /// Type of authentication
    /// </summary>
    public RequestAuthenticationType AuthenticationType { get; set; } = RequestAuthenticationType.Anonymous;

    /// <summary>
    /// Gets the time out. Default(30 seconds)
    /// </summary>
    /// <value>The time out.</value>
    public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(30);

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

    /// <summary>
    /// Gets the default headers.
    /// </summary>
    /// <value>The default headers.</value>
    public Dictionary<string, string> DefaultHeaders { get; set; } = [];

    /// <summary>
    /// Gets or sets the optional URL builder (Use for Dependency Injection)
    /// </summary>
    /// <value>The URL builder.</value>
    public Func<string, Uri> UrlBuilder { get; set; }


}
