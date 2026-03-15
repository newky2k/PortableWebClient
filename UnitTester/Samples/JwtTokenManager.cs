
using DSoft.Portable.WebClient.Rest;

namespace UnitTester.Samples;

internal class JwtTokenManager : IJwtTokenManger
{
    public async Task HandleAuthFailure(string key)
    {
        await Task.Delay(1000);
    }

    public Task<string> LoadAccessTokenAsync(string key)
    {
        return Task.FromResult("1234");
    }
}
