using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Providers;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency-injection helpers for registering the encryption services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the default encryption services, wiring <see cref="IIVKeyProvider"/> to the
    /// built-in IV key provider as a scoped service (only if not already registered).
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <returns>The same service collection, to allow chaining.</returns>
    public static IServiceCollection AddEncryptionProviders(this IServiceCollection services)
    {
        services.TryAddScoped<IIVKeyProvider, IVKeyProvider>();

        return services;
    }

}
