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

        var baseAddreess = "https://www.google.com";

        var sampleClient = serviceFactory.GetClient<ISampleRestService>(new Uri(baseAddreess), Guid.NewGuid().ToString());

        Assert.IsNotNull(sampleClient);
    }
}
