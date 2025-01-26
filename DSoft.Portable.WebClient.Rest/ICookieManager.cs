using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace DSoft.Portable.WebClient.Rest
{
    /// <summary>
    /// Interface for the cookie manager
    /// </summary>
    public interface ICookieManager
    {
        /// <summary>
        /// Validates the cookies and saves them
        /// </summary>
        void ValidateAndSave(string key);

        /// <summary>
        /// Saves the EFOS related cookies.
        /// </summary>
        void SaveCookies(string key);

        /// <summary>
        /// Load the EFOS related cookies from disk
        /// </summary>
        void LoadCookies(string key);

        /// <summary>
        /// Delete the stored EFOS cookies and expire the ones in memory
        /// </summary>
        void DeleteCookies(string key);

        /// <summary>
        /// Determines whether [has valid user cookies].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has valid user cookies]; otherwise, <c>false</c>.
        /// </returns>
        bool HasValidUserCookies { get; }
    }
}
