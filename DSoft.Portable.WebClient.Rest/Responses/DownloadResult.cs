
namespace DSoft.Portable.WebClient.Rest.Responses;

/// <summary>
/// Represents the result of a download operation, including the downloaded data and the associated file name.
/// </summary>
/// <remarks>Use this class to access both the raw data and the file name returned from a download process. This
/// is useful for saving the file to disk or further processing the downloaded content.</remarks>
public class DownloadResult
{
    /// <summary>
    /// Gets or sets the downloaded data as a byte array.
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    /// Gets or sets the name of the downloaded file.
    /// </summary>
    public string FileName { get; set; }
}
