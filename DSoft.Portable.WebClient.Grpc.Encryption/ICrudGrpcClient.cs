using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
	/// <summary>
	/// ICrudGrpcClient interface
	/// </summary>
	public interface ICrudGrpcClient
	{
		/// <summary>
		/// Adds the asynchronously.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="headers">The headers.</param>
		/// <param name="deadline">The deadline.</param>
		/// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
		AsyncUnaryCall<SecureResponse> AddAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Deletes the asynchronously.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="headers">The headers.</param>
		/// <param name="deadline">The deadline.</param>
		/// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
		AsyncUnaryCall<SecureResponse> DeleteAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Finds all asynchronously.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="headers">The headers.</param>
		/// <param name="deadline">The deadline.</param>
		/// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
		AsyncUnaryCall<SecureResponse> FindAllAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Finds the asynchronously.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="headers">The headers.</param>
		/// <param name="deadline">The deadline.</param>
		/// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
		AsyncUnaryCall<SecureResponse> FindAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Updates the asynchronously.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="headers">The headers.</param>
		/// <param name="deadline">The deadline.</param>
		/// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>AsyncUnaryCall&lt;SecureResponse&gt;.</returns>
		AsyncUnaryCall<SecureResponse> UpdateAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));



		}
}
