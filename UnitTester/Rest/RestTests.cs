using Microsoft.Extensions.DependencyInjection;
using UnitTester.Samples;

namespace UnitTester.Rest;

[TestClass]
public class RestTests : BaseTest
{
    [TestMethod]
    public async Task CanGetSampleClienty()
    {
        var sampleClient = Provider.GetRequiredService<ISampleRestService>();

        Assert.IsNotNull(sampleClient);

        var release = await sampleClient.GetReleaseAsync();
    }
}
