using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest
{
    /// <summary>
    /// Options for the rest client
    /// </summary>
    public class RestClientOptions
    {
        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public Uri BaseUrl { get; }

        /// <summary>
        /// Gets the client version no.
        /// </summary>
        /// <value>The client version no.</value>
        public string ClientVersionNo { get; }

        /// <summary>
        /// Gets the time out. Default(30 seconds)
        /// </summary>
        /// <value>The time out.</value>
        public TimeSpan TimeOut { get; } = TimeSpan.FromSeconds(30);
    }
}
