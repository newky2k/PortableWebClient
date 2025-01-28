using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Gets the client version no.
        /// </summary>
        /// <value>The client version no.</value>
        public string ClientVersionNo { get; set; }

        /// <summary>
        /// Gets the time out. Default(30 seconds)
        /// </summary>
        /// <value>The time out.</value>
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Gets or sets the optional cookie container.
        /// </summary>
        /// <value>
        /// The cookie container.
        /// </value>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// Gets or sets the an optional HTTP message handler. (Use for Unit Testing)
        /// </summary>
        /// <value>The HTTP message handler.</value>
        public HttpMessageHandler HttpMessageHandler { get; set; }
    }
}
