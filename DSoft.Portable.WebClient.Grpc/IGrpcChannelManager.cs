using System.Threading.Tasks;
using Grpc.Net.Client;

namespace DSoft.Portable.WebClient.Grpc;

/// <summary>
/// Manages the lifecycle of gRPC channels, caching one per address so callers reuse a single channel
/// rather than opening a new connection per call.
/// </summary>
public interface IGrpcChannelManager
{
    /// <summary>
    /// Returns the channel for the given address, creating and caching it on first use or returning the cached one thereafter.
    /// </summary>
    /// <param name="address">The server address the channel connects to.</param>
    /// <param name="options">The options controlling how the channel is created (HTTP mode, certificate handling, and so on).</param>
    /// <returns>A shared gRPC channel for the address.</returns>
    GrpcChannel ForAddress(string address, GrpcClientOptions options);

    /// <summary>
    /// Removes and shuts down the cached channel for the given address, so the next call recreates it.
    /// </summary>
    /// <param name="address">The server address whose channel should be cleared.</param>
    Task ClearAsync(string address);
}
