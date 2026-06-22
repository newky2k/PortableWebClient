using DSoft.Portable.WebClient.Encryption;

namespace DSoft.Portable.WebClient.Rest.Encryption;

/// <summary>
/// Base for REST clients that exchange encrypted payloads. Extends <see cref="RestServiceClientBase"/>
/// with the initialization vector and key size needed to build and read <see cref="SecurePayload"/>-based
/// requests and responses.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Rest.RestServiceClientBase" />
public abstract class RestServiceSecureClientBase : RestServiceClientBase
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
    /// The key size used by the cipher, taken from the secure client options.
    /// </summary>
    public KeySize KeySize => ((SecureRestApiClientOptions)Options).KeySize;


    #endregion

    #region Constructors

    /// <summary>
    /// Initializes the secure client with the IV provider and secure options used for encryption.
    /// </summary>
    /// <param name="initVectorProvider">Supplies the initialization vector for the cipher.</param>
    /// <param name="options">The secure client options, including the key size.</param>
    protected RestServiceSecureClientBase(IIVKeyProvider initVectorProvider, SecureRestApiClientOptions options) : base(options)
    {
        _initVectorProvider = initVectorProvider;
    }

    #endregion

}
