namespace DSoft.Portable.WebClient.Grpc;

/// <summary>
/// The HTTP protocol a gRPC channel uses, selecting between gRPC-Web over HTTP/1.1 and native gRPC over HTTP/2.
/// </summary>
public enum HttpMode
{
    /// <summary>
    /// HTTP/1.1, used for gRPC-Web (for example when calling through a browser-compatible endpoint or proxy).
    /// </summary>
    Http_1_1,
    /// <summary>
    /// HTTP/2, used for native gRPC.
    /// </summary>
    Http_2_0,
}
