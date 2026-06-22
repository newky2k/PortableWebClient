using System;
using System.Security.Cryptography;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// Placeholder payload used when a secure request or response carries no real data.
/// Encrypting random filler still produces a non-trivial cipher text, so empty exchanges
/// remain indistinguishable from ones that carry content.
/// </summary>
public class EmptyPayload
{
    /// <summary>
    /// The random filler string that stands in for an absent payload.
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// Builds a new instance populated with a random-length run of random bytes,
    /// so each empty payload differs from the last.
    /// </summary>
    public static EmptyPayload Empty
    {
        get
        {

            var rnd = new Random();
            var size = rnd.Next(5, 20);

            var data = new byte[size];

            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);

            var str = Encoding.UTF8.GetString(data);

            return new EmptyPayload()
            {
                Data = str,
            };
        }
    }
}
