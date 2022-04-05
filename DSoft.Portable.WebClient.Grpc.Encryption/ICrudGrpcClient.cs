using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
	public interface ICrudGrpcClient
	{
		AsyncUnaryCall<SecureResponse> AddAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

		AsyncUnaryCall<SecureResponse> DeleteAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

		AsyncUnaryCall<SecureResponse> FindAllAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

		AsyncUnaryCall<SecureResponse> FindAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));

		AsyncUnaryCall<SecureResponse> UpdateAsync(SecureRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));



		}
}
