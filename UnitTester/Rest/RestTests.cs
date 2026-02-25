using DSoft.Portable.WebClient.Rest;
using Microsoft.Extensions.DependencyInjection;
using SampleApiClient;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTester.Samples;

namespace UnitTester.Rest;

[TestClass]
public class RestTests : BaseTest
{

    [TestMethod]
    public void CanGetFactory()
    {
        var serviceFactory = Provider.GetRequiredService<IRestServiceClientFactory>();

        Assert.IsNotNull(serviceFactory);
    }

    [TestMethod]
    public async Task CanGetSampleClienty()
    {
        var serviceFactory = Provider.GetRequiredService<IRestServiceClientFactory>();

        Assert.IsNotNull(serviceFactory);

        var uniqueClientId = Guid.NewGuid().ToString();

        var sampleClient = serviceFactory.GetClient<ISampleRestService>(uniqueClientId);

        Assert.IsNotNull(sampleClient);

        await sampleClient.GetReleaseAsync();
    }
}
