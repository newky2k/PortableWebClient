
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace DSoft.Portable.WebClient.Rest;

internal class RestServiceClientFactory : IRestServiceClientFactory
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
            var baseAddress = options.UrlBuilder(uniqueId);

            restService.Configure(baseAddress, options, uniqueId);

            return client;
        }

        throw new Exception("Invalid base class. Must inherit from RestServiceClientBase");
    }
}
