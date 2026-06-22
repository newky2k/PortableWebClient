using System;
using System.Reflection;
using System.Threading.Tasks;
using DSoft.Portable.WebClient.Grpc;
using Microsoft.Extensions.Options;
using SampleRpc;

namespace SampleApiClient;

/// <summary>
/// Sample gRPC client showing how to derive from <see cref="GrpcServiceClientBase"/> and call a service
/// (here the generated <c>SampleContract</c> client) over the managed channel.
/// </summary>
public class SampleServiceClient : GrpcServiceClientBase
{
    #region Fields
    private Version _appVersion;
    #endregion

    /// <summary>
    /// The client version, read once from this assembly's version.
    /// </summary>
    protected override string ClientVersionNo
    {
        get
        {
            if (_appVersion == null)
            {
                var asm = Assembly.GetAssembly(this.GetType());

                _appVersion = asm.GetName().Version;
            }


            return _appVersion.ToString();
        }
    }

    /// <summary>
    /// Creates the client from DI-bound gRPC options.
    /// </summary>
    /// <param name="channelManager">The channel manager used to create and cache channels.</param>
    /// <param name="options">The configured gRPC client options.</param>
    public SampleServiceClient(IGrpcChannelManager channelManager, IOptions<GrpcClientOptions> options) : base(channelManager, options)
    {

    }

    /// <summary>
    /// Creates the client directly from a gRPC options instance.
    /// </summary>
    /// <param name="channelManager">The channel manager used to create and cache channels.</param>
    /// <param name="options">The gRPC client options.</param>
    public SampleServiceClient(IGrpcChannelManager channelManager, GrpcClientOptions options) : base(channelManager, options)
    {

    }

    /// <summary>
    /// Calls the sample service's <c>Find</c> method for the given id.
    /// </summary>
    /// <param name="id">The identifier to look up.</param>
    /// <returns>The service's response for that id.</returns>
    public async Task<SimpleResponse> FindAsync(int id)
    {

        var rpcClient = new SampleRpc.SampleContract.SampleContractClient(RPCChannel);

        var result = await rpcClient.FindAsync(new SampleRpc.SimpleRequest()
        {
            Id = id.ToString(),
        });


        return result;
    }
}
