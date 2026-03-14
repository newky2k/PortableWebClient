using System;
using System.Net.Http;
using DSoft.Portable.WebClient.Rest;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering REST service client factories and configuring REST API client options
/// within an IServiceCollection.
/// </summary>
/// <remarks>Use this class to enable dependency injection for REST API clients in your application. The extension
/// methods facilitate the registration of REST client factories and related configuration, supporting modular and
/// testable application design.</remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds REST service client logic to the specified service collection and configures REST API client options.
    /// </summary>
    /// <remarks>This method registers an implementation of IRestServiceClientFactory with scoped lifetime and
    /// configures RestApiClientOptions using the provided action. Use this extension method to enable dependency
    /// injection for REST API clients in your application.</remarks>
    /// <param name="services">The service collection to which the REST service client factory and related options will be added. Cannot be
    /// null.</param>
    /// <param name="configure">An action used to configure the options for the REST API client. Allows customization of settings such as base
    /// URL, timeouts, and other client behaviors. Cannot be null.</param>
    /// <returns>The service collection with the REST service client factory and options registered. Enables method chaining.</returns>
    public static IServiceCollection AddRestServiceClient(this IServiceCollection services, Action<RestApiClientOptions> configure)
    {
        services.AddOptions<RestApiClientOptions>().Configure(configure);

        return services;
    }


    /// <summary>
    /// Adds REST service client logic using IHttpClientFactory to the specified service collection and configures REST API client options.
    /// </summary>
    /// <remarks>This method registers an implementation of IRestServiceClientFactory with scoped lifetime and
    /// configures RestApiClientOptions using the provided action. Use this extension method to enable dependency
    /// injection for REST API clients in your application.</remarks>
    /// <param name="services">The service collection to which the REST service client factory and related options will be added. Cannot be
    /// null.</param>
    /// <param name="configure">An action used to configure the options for the REST API client. Allows customization of settings such as base
    /// URL, timeouts, and other client behaviors. Cannot be null.</param>
    /// <param name="configureHandler">Option handler configuration function</param>
    /// <returns>The service collection with the REST service client factory and options registered. Enables method chaining.</returns>
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
    /// Adds REST service client logic using IHttpClientFactory and registers the given Token Manager to the specified service collection and configures REST API client options.
    /// </summary>
    /// <remarks>This method registers an implementation of IRestServiceClientFactory with scoped lifetime and
    /// configures RestApiClientOptions using the provided action. Use this extension method to enable dependency
    /// injection for REST API clients in your application.</remarks>
    /// <param name="services">The service collection to which the REST service client factory and related options will be added. Cannot be
    /// null.</param>
    /// <param name="configure">An action used to configure the options for the REST API client. Allows customization of settings such as base
    /// URL, timeouts, and other client behaviors. Cannot be null.</param>
    /// <param name="configureHandler">Option handler configuration function</param>
    /// <param name="tokenManagerAsSingleton">Should the token manager use singleton lifecycle or scoped</param>
    /// <returns>The service collection with the REST service client factory and options registered. Enables method chaining.</returns>
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
