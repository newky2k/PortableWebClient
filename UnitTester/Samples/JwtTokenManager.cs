
using DSoft.Portable.WebClient.Rest;

namespace UnitTester.Samples;

internal class JwtTokenManager : IJwtTokenManger
{
    public Task<string> LoadAccessTokenAsync(string key) => Task.FromResult("1234");
}
