extern alias SUT; 
using DSoft.Portable.WebClient;
using DSoft.Portable.WebClient.Grpc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UnitTester.Samples;

namespace UnitTester
{
    [TestClass]
    public abstract class BaseTest
    {
        public static JsonSerializerOptions DefaultJsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        public static ServiceProvider Provider { get; private set; }

        public static WebApplicationFactory<SUT::SampleWebApp.Startup> webAppFactory { get; private set; }


        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Executes once before the test run. (Optional)
            webAppFactory = new WebApplicationFactory<SUT::SampleWebApp.Startup>();

            var services = new ServiceCollection();

            ConfigureServices(services);

            services.AddOptions().Configure<GrpcClientOptions>(x =>
            {
                x.GrpcMode = HttpMode.Http_1_1;
                x.DisableSSLCertValidation = true;
                x.UrlBuilder = () =>
                {
                    return webAppFactory.Server.BaseAddress.ToString();
                };
                x.HttpMessageHandler = webAppFactory.Server.CreateHandler();
            });

            services.AddRestServiceClientFactory(x =>
            {
                x.TimeOut = TimeSpan.FromSeconds(5);
                x.JsonSerializerOptions = DefaultJsonOptions;
                x.HttpMessageHandler = webAppFactory.Server.CreateHandler();
                x.UrlBuilder = (clientName) =>
                {
                    return webAppFactory.Server.BaseAddress;
                };
            });

            Provider = services.BuildServiceProvider();
        }

        public BaseTest()
        {
            
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IGrpcChannelManager, GrpcChannelManager>();
            services.TryAddScoped<SampleServiceClient>();

            //add sample rest service
            services.TryAddScoped<ISampleRestService, SampleRestService>(); 

            return services;
        }
    }
}
