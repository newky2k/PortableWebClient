using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Grpc
{
	/// <summary>
	/// GrpcChannelManager
	/// Implements the <see cref="DSoft.Portable.WebClient.Grpc.IGrpcChannelManager" />
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Grpc.IGrpcChannelManager" />
	public class GrpcChannelManager : IGrpcChannelManager
	{
		private readonly Dictionary<string, GrpcChannel> channels = new Dictionary<string, GrpcChannel>();

		/// <summary>
		/// Clears the channel for the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public async Task ClearAsync(string address)
		{
			if (channels.ContainsKey(address))
			{
				var channel = channels[address];

				await channel.ShutdownAsync();

				channels.Remove(address);

			}
		}

		/// <summary>
		/// Get the channel for the specified address, returning cached instance if found
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="options">The options.</param>
		/// <returns>GrpcChannel.</returns>
		/// <exception cref="System.Exception">Unexpected HTTP mode for Grpc Channel</exception>
		/// <exception cref="System.NotImplementedException"></exception>
		public GrpcChannel ForAddress(string address, GrpcClientOptions options)
		{
			if (channels.ContainsKey(address))
				return channels[address];

			GrpcChannel channel = null;

			switch (options.GrpcMode)
			{
				case HttpMode.Http_1_1:
					{
						//if a custom validator has be provided use that
						if (options.ServerCertificateCustomValidationCallback != null)
						{
							var httpClientHandlerCustom = new HttpClientHandler();
							httpClientHandlerCustom.ServerCertificateCustomValidationCallback = options.ServerCertificateCustomValidationCallback;

							channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
							{
								HttpHandler = new GrpcWebHandler(httpClientHandlerCustom)
							});
						}
						else if (options.DisableSSLCertValidation)
						{
							//return channel with SSL cert validation disabled
							var httpClientHandler = new HttpClientHandler();
#if NET6_0_OR_GREATER
							httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#else
                            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
#endif

							channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
							{
								HttpHandler = new GrpcWebHandler(httpClientHandler)
							});
						}
						else
						{
							//if disable SSL certifiate validation has not been set them return standard channel generator
							channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
							{
								HttpHandler = new GrpcWebHandler(new HttpClientHandler())
							});
						}
						break;
					}
				case HttpMode.Http_2_0:
					{
						//if a custom validator has be provided use that
						if (options.ServerCertificateCustomValidationCallback != null)
						{
							var httpClientHandlerCustom = new HttpClientHandler();
							httpClientHandlerCustom.ServerCertificateCustomValidationCallback = options.ServerCertificateCustomValidationCallback;

							channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions() { HttpClient = new HttpClient(httpClientHandlerCustom) });
						}
						else if (options.DisableSSLCertValidation)
						{
							//return channel with SSL cert validation disabled
							var httpClientHandler = new HttpClientHandler();
#if NET6_0_OR_GREATER
							httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#else
                            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
#endif
							var httpClient = new HttpClient(httpClientHandler);

							channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions() { HttpClient = httpClient });
						}
						else
						{
							//if disable SSL certifiate validation has not been set them return standard channel generator
							channel = GrpcChannel.ForAddress(address);
						}
						break;
					}
				default:
					throw new Exception("Unexpected HTTP mode for Grpc Channel");



			}

			channels.Add(address, channel);

			return channel;
		}
	}
}
