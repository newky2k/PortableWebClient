using System;
using DSoft.Portable.WebClient.Encryption.Providers;

namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// Encryption Provider Factory class for instantiating the registered encryption provider or the default Aes provider
/// </summary>
public class EncryptionProviderFactory
{
    private static Type _implementationType;

    /// <summary>
    /// Registers a custom provider type to use instead of the built-in AES provider.
    /// The most recent registration wins and applies to every subsequent <see cref="Build"/> call.
    /// </summary>
    /// <typeparam name="T">The <see cref="IEncryptionProvider"/> implementation to instantiate.</typeparam>
    public static void RegisterEncryptionProvider<T>() where T : IEncryptionProvider
    {
        _implementationType = typeof(T);
    }


    /// <summary>
    /// Creates a provider configured with the given initialization vector and key size,
    /// returning the registered custom provider if one was set, otherwise the built-in AES provider.
    /// </summary>
    /// <param name="initVector">The initialization vector string the provider will use.</param>
    /// <param name="keySize">The key size to derive; defaults to 256-bit.</param>
    /// <returns>A ready-to-use <see cref="IEncryptionProvider"/>.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="initVector"/> is null or whitespace.</exception>
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
