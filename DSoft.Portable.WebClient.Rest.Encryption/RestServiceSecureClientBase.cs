using DSoft.Portable.WebClient.Encryption;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
	/// <summary>
	/// Secure base service class
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Rest.RestServiceClientBase" />
	public abstract class RestServiceSecureClientBase : RestServiceClientBase
    {
        #region Fields
        private readonly IIVKeyProvider _initVectorProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the initialize vector.
        /// </summary>
        /// <value>The initialize vector.</value>
        public string InitVector => _initVectorProvider.InitVector;

        /// <summary>
        /// Gets the size of the key.
        /// </summary>
        /// <value>The size of the key.</value>
        public KeySize KeySize => ((SecureRestApiClientOptions)Options).KeySize;


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceSecureClientBase" /> class.
        /// </summary>
        /// <param name="initVectorProvider">The initialize vector provider.</param>
        protected RestServiceSecureClientBase(IIVKeyProvider initVectorProvider) : base()
        {
            _initVectorProvider = initVectorProvider;
        }

        #endregion

    }

}
