namespace DSoft.Portable.WebClient.Encryption;

/// <summary>
/// Supplies the initialization vector used to seed the encryption cipher, letting the IV
/// come from configuration or another source rather than being hard-coded.
/// </summary>
public interface IIVKeyProvider
{
    /// <summary>
    /// The initialization vector string the cipher should use.
    /// </summary>
    string InitVector { get; }

}
