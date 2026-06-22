using Microsoft.Extensions.Options;

namespace DSoft.Portable.WebClient.Encryption.Providers;

internal class IVKeyProvider : IIVKeyProvider
{
    #region Fields

    private readonly IVProviderOptions _options;

    #endregion
    #region Properties

    public string InitVector => _options.InitVector;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes the provider from DI-bound options.
    /// </summary>
    /// <param name="options">The configured <see cref="IVProviderOptions"/> resolved from the options pipeline.</param>
    public IVKeyProvider(IOptions<IVProviderOptions> options) : this(options.Value) { }

    /// <summary>
    /// Initializes the provider directly from an options instance.
    /// </summary>
    /// <param name="options">The options carrying the initialization vector to expose.</param>
    public IVKeyProvider(IVProviderOptions options)
    {
        _options = options;
    }

    #endregion
}
