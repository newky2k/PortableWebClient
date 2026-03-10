using DSoft.Portable.WebClient.Rest;

namespace UnitTester.Samples;

public interface ISampleRestService : IRestServiceClient
{
    Task<string> GetReleaseAsync();
}
