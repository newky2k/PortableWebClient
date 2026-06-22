
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Manages the lifecycle of authentication cookies for cookie-based clients: validating,
/// persisting, loading, and clearing the session cookies used on each request.
/// </summary>
public interface ICookieManager
{
    /// <summary>
    /// Validates the current cookies and, if valid, persists them under the given key.
    /// </summary>
    /// <param name="key">The key the cookies are stored under.</param>
    Task ValidateAndSaveAsync(string key);

    /// <summary>
    /// Persists the current session cookies under the given key.
    /// </summary>
    /// <param name="key">The key to store the cookies under.</param>
    Task SaveCookiesAsync(string key);

    /// <summary>
    /// Loads previously persisted cookies for the given key back into memory.
    /// </summary>
    /// <param name="key">The key the cookies were stored under.</param>
    Task LoadCookiesAsync(string key);

    /// <summary>
    /// Deletes the persisted cookies for the given key and expires any held in memory.
    /// </summary>
    /// <param name="key">The key the cookies were stored under.</param>
    Task DeleteCookiesAsync(string key);

    /// <summary>
    /// Whether valid, non-expired user cookies are currently available.
    /// </summary>
    bool HasValidUserCookies { get; }
}
