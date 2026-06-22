using System;
using System.Text.Json;

namespace DSoft.Portable.WebClient.Encryption.Helpers;

/// <summary>
/// Convenience helper that serializes objects to JSON and encrypts them (and the reverse),
/// wiring up an <see cref="IEncryptionProvider"/> via <see cref="EncryptionProviderFactory"/> for each call.
/// </summary>
public class PayloadManager
{
    private static TimeSpan _defaultTimeSpan = TimeSpan.FromMinutes(10);
    /// <summary>
    /// The validity window applied to time-stamped payloads. Defaults to 10 minutes.
    /// </summary>
    public static TimeSpan DefaultTimeout
    {
        get
        {
            return _defaultTimeSpan;
        }
        set
        {
            _defaultTimeSpan = value;
        }
    }

    /// <summary>
    /// Decrypts an encrypted payload string and deserializes it into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type the JSON payload represents.</typeparam>
    /// <param name="payload">The encrypted, Base64-encoded payload.</param>
    /// <param name="passkey">The pass phrase the payload was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The deserialized payload.</returns>
    public static T DecryptPayload<T>(string payload, string passkey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        var decryptedPayload = EncryptionProviderFactory.Build(initVector, keySize).DecryptString(payload, passkey);

        var outPut = JsonSerializer.Deserialize<T>(decryptedPayload);

        return outPut;
    }

    /// <summary>
    /// Serializes an object to JSON and encrypts it into a transportable string.
    /// </summary>
    /// <param name="payload">The object to serialize and encrypt.</param>
    /// <param name="passkey">The pass phrase to encrypt with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The encrypted, Base64-encoded payload.</returns>
    public static string EncryptPayload(object payload, string passkey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
    {
        var payloadString = JsonSerializer.Serialize(payload, payload.GetType());

        return EncryptionProviderFactory.Build(initVector, keySize).EncryptString(payloadString, passkey);
    }



}
