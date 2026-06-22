using System;

namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// An encrypted, time-stamped data container exchanged between client and server.
/// The payload can be checked for freshness and decrypted back into a typed object.
/// </summary>
public interface ISecurePayload
{
    /// <summary>
    /// The encrypted payload as a Base64-encoded string.
    /// </summary>
    string Data { get; set; }

    /// <summary>
    /// Checks whether the payload is still within its allowed age.
    /// </summary>
    /// <param name="timeSpan">The maximum age the payload may have.</param>
    /// <returns><c>true</c> when the payload is younger than <paramref name="timeSpan"/>; otherwise <c>false</c>.</returns>
    bool Validate(TimeSpan timeSpan);

    /// <summary>
    /// Decrypts the payload and deserializes it into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type the encrypted payload represents.</typeparam>
    /// <param name="passKey">The pass phrase the payload was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The decrypted, deserialized payload.</returns>
    T Extract<T>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
}
