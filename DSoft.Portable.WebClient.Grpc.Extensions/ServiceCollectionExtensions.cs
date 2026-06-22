using DSoft.Portable.WebClient.Grpc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency-injection helpers for registering the gRPC client services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the <see cref="IGrpcChannelManager"/> so gRPC channels are shared across clients
    /// (only if not already registered).
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="asSingleton">When <c>true</c> the manager is registered as a singleton (one channel cache for the app); otherwise scoped.</param>
    /// <returns>The same service collection, to allow chaining.</returns>
    public static IServiceCollection AddGrpcClientChannelManager(this IServiceCollection services, bool asSingleton = true)
    {
        if (asSingleton)
            services.TryAddSingleton<IGrpcChannelManager, GrpcChannelManager>();
        else
            services.TryAddScoped<IGrpcChannelManager, GrpcChannelManager>();

        return services;
    }
}
