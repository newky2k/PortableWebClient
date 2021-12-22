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
        /// Registers an encryption provider type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RegisterEncryptionProvider<T>() where T : IEncryptionProvider
        {
            _implementationType = typeof(T);
        }


        public static IEncryptionProvider Build(string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (string.IsNullOrWhiteSpace(initVector))
                throw new ArgumentNullException(nameof(initVector), "InitVector cannot be null or empty");


            if (_implementationType == null)
            {
                return new AesEncryptionProvider(initVector, keySize);
            }
            else
            {
                return Activator.CreateInstance(_implementationType, initVector, keySize) as IEncryptionProvider;
            }
        }
    }
}
