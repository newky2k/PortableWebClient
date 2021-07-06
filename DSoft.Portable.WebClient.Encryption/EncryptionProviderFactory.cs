using DSoft.Portable.WebClient.Encryption.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    /// <summary>
    /// Encryption Provider Factory class for instantiating the registered encryption provider or the default Aes provider
    /// </summary>
    public class EncryptionProviderFactory
    {
        private static Type _implementationType;

        /// <summary>
        /// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        /// </summary>
        /// <value>
        /// The initialize vector.
        /// </value>
        public static string InitVector { get; set; }


        /// <summary>
        /// Gets or sets the size of the key for the encryption
        /// </summary>
        /// <value>
        /// The size of the key.
        /// </value>
        public static KeySize KeySize { get; set; } = KeySize.TwoFiftySize;

        /// <summary>
        /// Registers an encryption provider type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RegisterEncryptionProvider<T>() where T : EncryptionProviderBase
        {
            _implementationType = typeof(T);
        }

        /// <summary>
        /// Builds this encryption provider with the configured InitVector and KeySize
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">EncryptionProviderFactory.InitVector cannot be null or empty</exception>
        public static IEncryptionProvider Build()
        {
            if (string.IsNullOrWhiteSpace(InitVector))
                throw new ArgumentNullException("EncryptionProviderFactory.InitVector cannot be null or empty");


            if (_implementationType == null)
            {
                return new AesEncryptionProvider(InitVector, KeySize);
            }              
            else
            {
                return Activator.CreateInstance(_implementationType, InitVector, KeySize) as IEncryptionProvider;
            }
        }

    }
}
