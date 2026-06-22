using System;
using System.Threading;
using Grpc.Core;

namespace DSoft.Portable.WebClient.Grpc.Encryption;

/// <summary>
/// Contract for a generated gRPC client exposing the standard CRUD operations, each taking an encrypted
/// <see cref="SecureRequest"/> and returning an encrypted <see cref="SecureResponse"/>.
/// </summary>
public interface ICrudGrpcClient
{
    /// <summary>
    /// Creates a new entity from the encrypted request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="deadline">The deadline.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
    AsyncUnaryCall<SecureResponse> AddAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Deletes the entity identified by the encrypted request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="deadline">The deadline.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
    AsyncUnaryCall<SecureResponse> DeleteAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Retrieves all matching entities for the encrypted request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="deadline">The deadline.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
    AsyncUnaryCall<SecureResponse> FindAllAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Retrieves a single entity identified by the encrypted request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="deadline">The deadline.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
    AsyncUnaryCall<SecureResponse> FindAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Updates an existing entity from the encrypted request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="headers">The headers.</param>
    /// <param name="deadline">The deadline.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
    AsyncUnaryCall<SecureResponse> UpdateAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));



}
