using DSoft.Portable.WebClient.Encryption.Providers;

namespace DSoft.Portable.WebClient.Encryption.Factories;

/// <summary>
/// Creates <see cref="IIVKeyProvider"/> instances from options, for callers that need an IV
/// provider outside of dependency injection.
/// </summary>
public static class IVProviderFactory
{
    /// <summary>
    /// Builds an IV key provider backed by the supplied options.
    /// </summary>
    /// <param name="options">The options carrying the initialization vector.</param>
    /// <returns>A ready-to-use <see cref="IIVKeyProvider"/>.</returns>
    public static IIVKeyProvider Create(IVProviderOptions options) => new IVKeyProvider(options);
}
