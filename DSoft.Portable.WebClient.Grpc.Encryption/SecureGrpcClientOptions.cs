using DSoft.Portable.WebClient.Encryption;

namespace DSoft.Portable.WebClient.Grpc.Encryption;

/// <summary>
/// <see cref="GrpcClientOptions"/> extended with the encryption settings a secure gRPC client needs.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Grpc.GrpcClientOptions" />
public class SecureGrpcClientOptions : GrpcClientOptions
{
    /// <summary>
    /// The key size the cipher should use when encrypting and decrypting payloads.
    /// </summary>
    public KeySize KeySize { get; }
}
