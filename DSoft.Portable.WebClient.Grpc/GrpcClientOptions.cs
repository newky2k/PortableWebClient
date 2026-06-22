using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace DSoft.Portable.WebClient.Grpc;

/// <summary>
/// Configuration for a gRPC client: the HTTP mode, TLS certificate handling, address resolution, and an
/// optional message handler for testing.
/// </summary>
public class GrpcClientOptions
{
    /// <summary>
    /// Http mode for Grpc channels.  1.1 will use Grpc-web, 2.0 will use HTTP/2
    /// </summary>
    public HttpMode GrpcMode { get; set; } = HttpMode.Http_1_1;

    /// <summary>
    /// Disable the SSL Certificate validation.  Ignored if ServerCertificateCustomValidationCallback is set
    /// </summary>
    public bool DisableSSLCertValidation { get; set; } = false;

    /// <summary>
    /// Callback function to override defualt SSL Certificate validatiion.  If set the DisableSSLCertValidation is ignored
    /// </summary>
    public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> ServerCertificateCustomValidationCallback { get; set; }

    /// <summary>
    /// Optional function that resolves the server address at runtime, allowing the endpoint to be supplied
    /// via dependency injection rather than hard-coded.
    /// </summary>
    public Func<string> UrlBuilder { get; set; }

    /// <summary>
    /// An optional HTTP message handler to inject into the channel; primarily used to route calls through a
    /// test server (for example <c>WebApplicationFactory</c>).
    /// </summary>
    public HttpMessageHandler HttpMessageHandler { get; set; }

}
