using System;
using System.Net.Http;
using DSoft.Portable.WebClient.Rest;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency-injection helpers for registering REST clients and their <see cref="RestApiClientOptions"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="RestApiClientOptions"/> configured by the supplied action, for clients that
    /// manage their own <see cref="System.Net.Http.HttpClient"/>.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="configure">Action that configures the client options (base URL, timeouts, auth, and so on).</param>
    /// <returns>The same service collection, to allow chaining.</returns>
    public static IServiceCollection AddRestServiceClient(this IServiceCollection services, Action<RestApiClientOptions> configure)
    {
        services.AddOptions<RestApiClientOptions>().Configure(configure);

        return services;
    }


    /// <summary>
    /// Registers <see cref="RestApiClientOptions"/> and a typed <c>HttpClient</c> for <c>PortableRestHttpClient</c>
    /// through <c>IHttpClientFactory</c>, optionally overriding the primary HTTP message handler.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="configure">Action that configures the client options (base URL, timeouts, auth, and so on).</param>
    /// <param name="configureHandler">Optional factory for the primary HTTP message handler; useful for tests or custom transport.</param>
    /// <returns>The same service collection, to allow chaining.</returns>
    public static IServiceCollection AddRestServiceClientWithFactory(this IServiceCollection services, Action<RestApiClientOptions> configure, Func<HttpMessageHandler> configureHandler = null)
    {
        services.AddOptions<RestApiClientOptions>().Configure(configure);

        var httpClientBuilder = services.AddHttpClient<PortableRestHttpClient>(c => { });

        if (configureHandler is not null)
        {
            httpClientBuilder.ConfigurePrimaryHttpMessageHandler(configureHandler);
        }

        return services;
    }

    /// <summary>
    /// Same as <see cref="AddRestServiceClientWithFactory(IServiceCollection, Action{RestApiClientOptions}, Func{HttpMessageHandler})"/>
    /// but also registers the given <see cref="IJwtTokenManger"/> implementation for token-based authentication.
    /// </summary>
    /// <typeparam name="T">The token manager implementation to register.</typeparam>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="configure">Action that configures the client options (base URL, timeouts, auth, and so on).</param>
    /// <param name="configureHandler">Optional factory for the primary HTTP message handler; useful for tests or custom transport.</param>
    /// <param name="tokenManagerAsSingleton">When <c>true</c> the token manager is registered as a singleton; otherwise scoped.</param>
    /// <returns>The same service collection, to allow chaining.</returns>
    public static IServiceCollection AddRestServiceClientWithFactory<T>(this IServiceCollection services, Action<RestApiClientOptions> configure, Func<HttpMessageHandler> configureHandler = null, bool tokenManagerAsSingleton = true)
        where T : class, IJwtTokenManger
    {

        if (tokenManagerAsSingleton)
        {
            services.TryAddSingleton<IJwtTokenManger, T>();
        }
        else
        {
            services.TryAddScoped<IJwtTokenManger, T>();
        }

        services.AddOptions<RestApiClientOptions>().Configure(configure);

        var httpClientBuilder = services.AddHttpClient<PortableRestHttpClient>(c => { });

        if (configureHandler is not null)
        {
            httpClientBuilder.ConfigurePrimaryHttpMessageHandler(configureHandler);
        }

        return services;
    }
}
