using DSoft.Portable.WebClient.Rest;
using DSoft.Portable.WebClient.Rest.Enums;

namespace UnitTester.Samples;

internal class SampleRestService : RestServiceClientBase, ISampleRestService
{
    public override string ClientVersionNo => "1.0";

    protected override string ControllerName => "Release";

    public SampleRestService()
    {
        
    }

    public async Task<ReleaseInfo> GetReleaseAsync()
    {
        var result = await ExecuteGetAsync<ReleaseInfo>("Current", authentication: RequestAuthenticationType.Anonymous);

        return result;
    }
}

public class ReleaseInfo()
{
    string Release { get; set; }
}
