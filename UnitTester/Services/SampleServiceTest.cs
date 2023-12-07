using Microsoft.Extensions.DependencyInjection;
using SampleApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTester.Services
{
    [TestClass]
    public class SampleServiceTest : BaseTest
    {
        public SampleServiceTest() : base()
        {
                
        }

        [TestMethod]
        public void CanLoadServiceClient()
        {
            var sampleServiceClient = Provider.GetRequiredService<SampleServiceClient>();

            Assert.IsNotNull(sampleServiceClient);

        }

        [TestMethod]
        public async Task CanCallFunctionClientAsync()
        {
            var sampleServiceClient = Provider.GetRequiredService<SampleServiceClient>();

            Assert.IsNotNull(sampleServiceClient);

            await sampleServiceClient.FindAsync(1);
        }
    }
}
