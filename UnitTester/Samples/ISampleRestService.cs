using DSoft.Portable.WebClient.Rest;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTester.Samples;

public interface ISampleRestService : IRestServiceClient
{
    Task<ReleaseInfo> GetReleaseAsync();
}
