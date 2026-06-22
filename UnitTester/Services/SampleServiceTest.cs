using Microsoft.Extensions.DependencyInjection;
using SampleApiClient;

namespace UnitTester.Services;

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
