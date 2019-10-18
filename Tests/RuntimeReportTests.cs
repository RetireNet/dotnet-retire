using System.Threading.Tasks;
using RetireRuntimeMiddleware.HttpClients;
using Xunit;

namespace Tests
{
    public class RuntimeReportTests
    {

        [Fact]
        public async Task TestReport()
        {
            var client = new ReleaseMetadataClient();
            var report = await client.GetReport();
            var hasRuntimeVersion = !string.IsNullOrEmpty(report.AppRuntimeDetails.RuntimeVersion);
            Assert.True(hasRuntimeVersion);
        }
    }
}
