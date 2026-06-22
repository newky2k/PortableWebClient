using System;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace DSoft.Portable.WebClient.Grpc;

/// <summary>
/// Abstract base for gRPC service clients. Resolves the server address from options and obtains a shared
/// channel from the <see cref="IGrpcChannelManager"/>, exposing it to derived clients.
/// </summary>
/// <seealso cref="System.IDisposable" />
public abstract class GrpcServiceClientBase : IDisposable
{
    #region Fields
    private GrpcClientOptions _options;
    private readonly IGrpcChannelManager _grpcChannelManager;

    #endregion

    #region Properties

    /// <summary>
    /// The options this client was configured with.
    /// </summary>
    protected GrpcClientOptions Options => _options;

    /// <summary>
    /// The gRPC channel for the configured address, created or reused via the channel manager on each access.
    /// </summary>
    /// <exception cref="System.Exception">Thrown when the options specify an unsupported HTTP mode.</exception>
    protected GrpcChannel RPCChannel => _grpcChannelManager.ForAddress(_options.UrlBuilder(), _options);

    /// <summary>
    /// The channel manager used to create and cache channels.
    /// </summary>
    protected IGrpcChannelManager GrpcChannelManager => _grpcChannelManager;

    /// <summary>
    /// The client version string sent to the server. Supplied by the concrete client.
    /// </summary>
    abstract protected string ClientVersionNo { get; }

    #endregion

    /// <summary>
    /// Initializes the client from DI-bound options.
    /// </summary>
    /// <param name="grpcChannelManager">The channel manager used to create and cache channels.</param>
    /// <param name="options">The configured gRPC client options resolved from the options pipeline.</param>
    protected GrpcServiceClientBase(IGrpcChannelManager grpcChannelManager, IOptions<GrpcClientOptions> options) : this(grpcChannelManager, options.Value)
    {

    }

    /// <summary>
    /// Initializes the client directly from an options instance.
    /// </summary>
    /// <param name="grpcChannelManager">The channel manager used to create and cache channels.</param>
    /// <param name="options">The gRPC client options; if null, defaults are used.</param>
    protected GrpcServiceClientBase(IGrpcChannelManager grpcChannelManager, GrpcClientOptions options)
    {
        _grpcChannelManager = grpcChannelManager;


        //set default options
        _options = (options == null) ? new GrpcClientOptions() : options;
    }

    /// <summary>
    /// Releases resources held by the client. No unmanaged resources are held today, so this is a no-op
    /// hook that derived clients may override.
    /// </summary>
    public virtual void Dispose()
    {

    }
}

/// <summary>
/// Strongly-typed gRPC client base that creates the generated gRPC client <typeparamref name="T"/> over the
/// managed channel, so derived clients can call service methods directly through <see cref="Client"/>.
/// </summary>
/// <typeparam name="T">The generated gRPC client type to instantiate over the channel.</typeparam>
/// <seealso cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
public abstract class GrpcServiceClientBase<T> : GrpcServiceClientBase
    where T : ClientBase
{

    /// <summary>
    /// The generated gRPC client, constructed over the current <see cref="GrpcServiceClientBase.RPCChannel"/>.
    /// </summary>
    protected T Client
    {
        get
        {
            var rpcClient = Activator.CreateInstance(typeof(T), new object[] { RPCChannel });

            return (T)rpcClient;
        }
    }

    /// <summary>
    /// Initializes the typed client from DI-bound options.
    /// </summary>
    /// <param name="grpcChannelManager">The channel manager used to create and cache channels.</param>
    /// <param name="options">The configured gRPC client options resolved from the options pipeline.</param>
    protected GrpcServiceClientBase(IGrpcChannelManager grpcChannelManager, IOptions<GrpcClientOptions> options) : base(grpcChannelManager, options.Value) { }

    /// <summary>
    /// Initializes the typed client directly from an options instance.
    /// </summary>
    /// <param name="grpcChannelManager">The channel manager used to create and cache channels.</param>
    /// <param name="options">The gRPC client options; if null, defaults are used.</param>
    protected GrpcServiceClientBase(IGrpcChannelManager grpcChannelManager, GrpcClientOptions options) : base(grpcChannelManager, options) { }

}
