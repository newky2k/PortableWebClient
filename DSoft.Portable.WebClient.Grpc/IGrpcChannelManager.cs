using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Grpc
{
	/// <summary>
	/// IGrpcChannelManager Interface
	/// </summary>
	public interface IGrpcChannelManager
	{
		/// <summary>
		/// Get the channel for the specified address, returning cached instance if found
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="options">The options.</param>
		/// <returns>GrpcChannel.</returns>
		GrpcChannel ForAddress(string address, GrpcClientOptions options);

		/// <summary>
		/// Clears the channel for the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		Task ClearAsync(string address);
	}
}
