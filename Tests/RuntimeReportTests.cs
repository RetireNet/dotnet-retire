using System.Threading.Tasks;
using RetireRuntimeMiddleware;
using RetireRuntimeMiddleware.Clients;
using RetireRuntimeMiddleware.HttpClients;
using Xunit;

namespace Tests
{
    public class RuntimeReportTests
    {

        [Fact]
        public async Task TestReport()
        {
            var client = new ReportGenerator();
            var report = await client.GetReport();
            var hasRuntimeVersion = !string.IsNullOrEmpty(report.AppRuntimeDetails.AppRuntimeVersion);
            Assert.True(hasRuntimeVersion);
        }
    }
}
