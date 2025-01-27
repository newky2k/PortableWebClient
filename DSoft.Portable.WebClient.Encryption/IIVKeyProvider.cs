using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    /// <summary>
    /// Interface Init Vector Provider
    /// </summary>
    public interface IIVKeyProvider
    {
        /// <summary>
        /// Gets the init vertor for the encryption.
        /// </summary>
        /// <value>The key.</value>
        string InitVector { get; }

    }
}
