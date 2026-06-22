using DSoft.Portable.WebClient.Encryption;
using Microsoft.Extensions.Options;

namespace DSoft.Portable.WebClient.Grpc.Encryption;

/// <summary>
/// Base for gRPC clients that exchange encrypted payloads. Extends <see cref="GrpcServiceClientBase"/>
/// with the initialization vector and key size needed to encrypt and decrypt secure payloads.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Grpc.GrpcServiceClientBase" />
public abstract class GrpcServiceSecureClientBase : GrpcServiceClientBase
{
    #region Fields
    private readonly IIVKeyProvider _initVectorProvider;

    #endregion

    #region Properties

    /// <summary>
    /// The initialization vector used to encrypt and decrypt this client's payloads.
    /// </summary>
    public string InitVector => _initVectorProvider.InitVector;

    /// <summary>
    /// The key size used by the cipher, taken from the secure gRPC options.
    /// </summary>
    public KeySize KeySize => ((SecureGrpcClientOptions)Options).KeySize;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes the secure client directly from a secure options instance.
    /// </summary>
    /// <param name="grpcChannelManager">The channel manager used to create and cache channels.</param>
    /// <param name="options">The secure gRPC options, including the key size.</param>
    /// <param name="initVectorProvider">Supplies the initialization vector for the cipher.</param>
    protected GrpcServiceSecureClientBase(IGrpcChannelManager grpcChannelManager, SecureGrpcClientOptions options, IIVKeyProvider initVectorProvider) : base(grpcChannelManager, options)
    {
        _initVectorProvider = initVectorProvider;
    }

    /// <summary>
    /// Initializes the secure client from DI-bound secure options.
    /// </summary>
    /// <param name="grpcChannelManager">The channel manager used to create and cache channels.</param>
    /// <param name="options">The configured secure gRPC options resolved from the options pipeline.</param>
    /// <param name="initVectorProvider">Supplies the initialization vector for the cipher.</param>
    protected GrpcServiceSecureClientBase(IGrpcChannelManager grpcChannelManager, IOptions<SecureGrpcClientOptions> options, IIVKeyProvider initVectorProvider) : base(grpcChannelManager, options)
    {
        _initVectorProvider = initVectorProvider;
    }

    #endregion
}
