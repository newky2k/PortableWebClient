
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// JWT Token Manager
/// </summary>
public interface IJwtTokenManger
{
    /// <summary>
    /// Load the JWT Access Token for the specified url or unique connection id
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<string> LoadAccessTokenAsync(string key);

    /// <summary>
    /// This is called when there is an authentication failure with the provided access token.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task HandleAuthFailure(string key);

}
