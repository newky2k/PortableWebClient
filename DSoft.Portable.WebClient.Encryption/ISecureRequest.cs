using System;

namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// A request envelope that carries an encrypted <see cref="ISecurePayload"/> together with the
/// identifiers a server needs to route and authorize it.
/// </summary>
/// <typeparam name="T">The concrete secure payload type carried by the request.</typeparam>
public interface ISecureRequest<T> where T : ISecurePayload
{
    /// <summary>
    /// A correlation identifier for the request.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// The identifier of the authentication token the request is made under.
    /// </summary>
    string TokenId { get; set; }

    /// <summary>
    /// An optional caller-defined context string passed alongside the request.
    /// </summary>
    string Context { get; set; }

    /// <summary>
    /// The encrypted payload being sent.
    /// </summary>
    T Payload { get; set; }

    /// <summary>
    /// Validates the request's payload against the given freshness window.
    /// </summary>
    /// <param name="timeout">The maximum age the payload may have.</param>
    void Validate(TimeSpan timeout);

    /// <summary>
    /// Decrypts the request's payload and deserializes it into <typeparamref name="TData"/>.
    /// </summary>
    /// <typeparam name="TData">The type the encrypted payload represents.</typeparam>
    /// <param name="passKey">The pass phrase the payload was encrypted with.</param>
    /// <param name="initVector">The initialization vector used by the cipher.</param>
    /// <param name="keySize">The key size used by the cipher; defaults to 256-bit.</param>
    /// <returns>The decrypted, deserialized payload.</returns>
    TData ExtractPayload<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
}
