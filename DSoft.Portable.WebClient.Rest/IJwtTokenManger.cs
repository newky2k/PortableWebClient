
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Supplies JWT access tokens to a token-authenticated client and reacts to authentication
/// failures (for example by clearing or refreshing a stale token).
/// </summary>
public interface IJwtTokenManger
{
    /// <summary>
    /// Loads the JWT access token to use for the given URL or connection key.
    /// </summary>
    /// <param name="key">The URL or unique connection identifier the token is scoped to.</param>
    /// <returns>The bearer token to send, or null if none is available.</returns>
    Task<string> LoadAccessTokenAsync(string key);

    /// <summary>
    /// Called when a request fails authentication with the supplied token, giving the manager a
    /// chance to invalidate or refresh the stored token for the given key.
    /// </summary>
    /// <param name="key">The URL or unique connection identifier the failed token was scoped to.</param>
    Task HandleAuthFailureAsync(string key);

}
