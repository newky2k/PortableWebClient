using DSoft.Portable.WebClient.Encryption.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption.Factories
{
    /// <summary>
    /// Factory for the IV Provider
    /// </summary>
    public static class IVProviderFactory
    {
        /// <summary>
        /// Creates the IV key provider with the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>IIVKeyProvider.</returns>
        public static IIVKeyProvider Create(IVProviderOptions options) => new IVKeyProvider(options);
    }
}
