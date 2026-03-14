
using DSoft.Portable.WebClient.Rest;

namespace UnitTester.Samples;

internal class JwtTokenManager : IJwtTokenManger
{
    public Task<string> LoadAccessTokenAsync(string key)
    {
        return Task.FromResult("1234");
    }
}
