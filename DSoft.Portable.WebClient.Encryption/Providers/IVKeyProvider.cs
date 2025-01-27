using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption.Providers
{
    internal class IVKeyProvider : IIVKeyProvider
    {
        #region Fields

        private readonly IVProviderOptions _options;

        #endregion
        #region Properties

        public string InitVector => _options.InitVector;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IVKeyProvider"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public IVKeyProvider(IOptions<IVProviderOptions> options) : this(options.Value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IVKeyProvider"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public IVKeyProvider(IVProviderOptions options)
        {
            _options = options;
        }

        #endregion
    }
}
