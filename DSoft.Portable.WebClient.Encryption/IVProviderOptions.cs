namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// Options used to configure an <see cref="IIVKeyProvider"/>, carrying the initialization vector
/// to expose to the encryption pipeline.
/// </summary>
public class IVProviderOptions
{
    /// <summary>
    /// The initialization vector string the provider will expose.
    /// </summary>
    public string InitVector { get; set; }

}
