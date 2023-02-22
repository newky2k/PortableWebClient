using DSoft.Portable.WebClient.Grpc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// IServiceCollection Extensions.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the GRPC channel manager.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <param name="asSingleton">if set to <c>true</c> [as singleton].</param>
		/// <returns>IServiceCollection.</returns>
		public static IServiceCollection AddGrpcClientChannelManager(this IServiceCollection services, bool asSingleton = true)
		{
			if (asSingleton)
				services.TryAddSingleton<IGrpcChannelManager, GrpcChannelManager>();
			else
				services.TryAddScoped<IGrpcChannelManager, GrpcChannelManager>();

			return services;
		}
	}
}