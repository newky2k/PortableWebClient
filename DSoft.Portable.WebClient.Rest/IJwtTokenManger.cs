
namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// JWT Token Manager
/// </summary>
public interface IJwtTokenManger
{
    /// <summary>
    /// Load the JWT Access Token for the specified url or unique connection id
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    string LoadAccessToken(string uniqueId);

}
