using System.Threading.Tasks;
using RetireRuntimeMiddleware.Clients;
using RetireRuntimeMiddleware.Clients.Models;
using RetireRuntimeMiddleware.HttpClients;
using Xunit;

namespace Tests
{
    public class RuntimeReportIntegrationTests
    {
        [Theory]
        [InlineData("2.1.11", true)]
        [InlineData("2.1.13", false)]
        [InlineData("3.0.0", false)]
        [InlineData("3.1.0-preview1.19506.1", false)]
        public async Task VulnerabilityReports(string version, bool isVulnerable)
        {
            var releaseMetadataClient = new ReleaseMetadataClient();
            var client = new ReportGenerator(releaseMetadataClient);
            var report = await client.GetReport(AppRunTimeDetails.Build(version));
            Assert.Equal(isVulnerable, report.IsVulnerable);
        }

        [Fact]
        public async Task UnknownRuntimes()
        {
            var releaseMetadataClient = new ReleaseMetadataClient();
            var client = new ReportGenerator(releaseMetadataClient);
            var report = await client.GetReport(AppRunTimeDetails.Build("abc"));
            Assert.Null(report.IsVulnerable);
            Assert.Equal($"Running on unknown runtime abc. Not able to check for security patches.", report.Details);
        }

        [Fact(Skip = "Testing all releases. Slow.")]

        public async Task CanGetReportForAllRuntimes()
        {
            var httpClient = new ReleaseMetadataHttpClient();
            var allChannels = await httpClient.GetAllChannelsAsync();
            foreach (var channel in allChannels)
            {
                foreach (var release in channel.Releases)
                {
                    var releaseMetadataClient = new ReleaseMetadataClient();
                    var client = new ReportGenerator(releaseMetadataClient);
                    if (release.Runtime != null)
                    {
                        var report = await client.GetReport(AppRunTimeDetails.Build(release.Runtime.Version));
                        Assert.NotNull(report);
                    }
                }
            }
        }
    }
}
