using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Encryption Provider Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the encryption providers.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddEncryptionProviders(this IServiceCollection services)
        {
            services.TryAddScoped<IIVKeyProvider, IVKeyProvider>();

            return services;
        }

    }
}
