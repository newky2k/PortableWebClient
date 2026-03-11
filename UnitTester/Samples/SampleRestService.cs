using DSoft.Portable.WebClient.Rest;
using DSoft.Portable.WebClient.Rest.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace UnitTester.Samples;

internal class SampleRestService : RestServiceClientBase, ISampleRestService
{
    public override string ClientVersionNo => "1.0";

    protected override string ControllerName => "Release";

    protected override string ApiPrefix => "api";

    public SampleRestService(IOptions<RestApiClientOptions> options, PortableRestHttpClient httpClient, IServiceScopeFactory serviceScopeFactory) : base(options, httpClient, serviceScopeFactory) { }

    public async Task<string> GetReleaseAsync()
    {
        var result = await ExecuteGetAsync<string>("Current", authentication: RequestAuthenticationType.Anonymous);

        return result;
    }

    public override Task<string> GetUniqueIdAsync() => Task.FromResult("TestClient");
}

public class ReleaseInfo()
{
    string Release { get; set; }
}
