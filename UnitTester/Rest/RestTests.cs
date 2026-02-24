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
    public void CanGetSampleClienty()
    {
        var serviceFactory = Provider.GetRequiredService<IRestServiceClientFactory>();

        Assert.IsNotNull(serviceFactory);

        var sampleClient = serviceFactory.GetClient<ISampleRestService>(Guid.NewGuid().ToString());

        Assert.IsNotNull(sampleClient);
    }
}
