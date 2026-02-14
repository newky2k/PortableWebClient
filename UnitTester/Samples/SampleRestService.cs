using DSoft.Portable.WebClient.Core.Enum;
using DSoft.Portable.WebClient.Rest;
using DSoft.Portable.WebClient.Rest.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

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
        var result = await ExecuteGetAsync<ReleaseInfo>("Current", RequestAuthenticationType.None);

        return result;
    }
}

public class ReleaseInfo()
{
    string Release { get; set; }
}
