
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Interface for the cookie manager
/// </summary>
public interface ICookieManager
{
    /// <summary>
    /// Validates the cookies and saves them
    /// </summary>
    Task ValidateAndSaveAsync(string key);

    /// <summary>
    /// Saves the EFOS related cookies.
    /// </summary>
    Task SaveCookiesAsync(string key);

    /// <summary>
    /// Load the EFOS related cookies from disk
    /// </summary>
    Task LoadCookiesAsync(string key);

    /// <summary>
    /// Delete the stored EFOS cookies and expire the ones in memory
    /// </summary>
    Task DeleteCookiesAsync(string key);

    /// <summary>
    /// Determines whether [has valid user cookies].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [has valid user cookies]; otherwise, <c>false</c>.
    /// </returns>
    bool HasValidUserCookies { get; }
}
