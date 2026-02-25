
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace DSoft.Portable.WebClient.Rest;

internal sealed class RestServiceClientFactory : IRestServiceClientFactory
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly RestApiClientOptions options;

    public RestServiceClientFactory(IServiceScopeFactory scopeFactory, IOptions<RestApiClientOptions> options)
    {
        this.scopeFactory = scopeFactory;
        this.options = options?.Value ?? new();
    }

    public T GetClient<T>(string uniqueId = null) where T : IRestServiceClient
    {
        using var scope = scopeFactory.CreateScope();

        var client = scope.ServiceProvider.GetService<T>();

        if (client is RestServiceClientBase restService)
        {
            if (options.UrlBuilder == null)
                throw new InvalidOperationException("You must either set the Url Build handler on RestApiClientOptions or use GetClient<T>(Uri, String) instead");


            var baseAddress = options.UrlBuilder(uniqueId);

            restService.Configure(baseAddress, options, uniqueId);

            return client;
        }

        throw new InvalidOperationException("Invalid base class. Must inherit from RestServiceClientBase");
    }

    public T GetClient<T>(Uri baseAddress, string uniqueId = null) where T : IRestServiceClient
    {
        using var scope = scopeFactory.CreateScope();

        var client = scope.ServiceProvider.GetService<T>();

        if (client is RestServiceClientBase restService)
        {
            restService.Configure(baseAddress, options, uniqueId);

            return client;
        }

        throw new InvalidOperationException("Invalid base class. Must inherit from RestServiceClientBase");
    }
}
