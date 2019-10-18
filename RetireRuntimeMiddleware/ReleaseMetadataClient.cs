using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RetireRuntimeMiddleware.HttpClients.Models;
using RetireRuntimeMiddleware.Middlewares.Models;

namespace RetireRuntimeMiddleware.HttpClients
{
    internal class ReleaseMetadataClient
    {
        private ReleaseMetadataHttpClient _httpClient;

        public ReleaseMetadataClient()
        {
            _httpClient = new ReleaseMetadataHttpClient();
        }

        public async Task<Report> GetReport()
        {
            var appRunTimeDetails = AppRunTimeDetails.New();
            var runtimes = await GetVulnerableRuntimes();
            var vulnerableRuntime = runtimes.Find(appRunTimeDetails.RuntimeVersion);

            return new Report
            {
                AppRuntimeDetails = appRunTimeDetails,
                vulnerable = vulnerableRuntime != null,
                VulnerableRuntimeInfo = vulnerableRuntime,
            };
        }

        private async Task<VulnerabilityReport> GetVulnerableRuntimes()
        {
            var vulnerabilityReport = new VulnerabilityReport();
            var index = await _httpClient.GetIndexAsync();

            var tasks = new List<Task<IEnumerable<Release>>>();

            foreach (var channel in index.Channels)
            {
                tasks.Add(GetVulnerableRuntimesForChannel(channel));
            }

            var allReleasesAcrossChannels = await Task.WhenAll(tasks);

            foreach (var releasesForSpecificChannel in allReleasesAcrossChannels)
            {
                vulnerabilityReport.Add(releasesForSpecificChannel);
            }

            return vulnerabilityReport;
        }

        private async Task<IEnumerable<Release>> GetVulnerableRuntimesForChannel(Channel channel)
        {
            var releasesContainer = await _httpClient.GetReleases(channel.ReleasesUrl);

            var vulnerableReleases = new List<Release>();

            foreach (var release in releasesContainer.Releases)
            {
                if (release.CVEs == null)
                    continue;

                if (release.CVEs.Any())
                    vulnerableReleases.Add(release);
            }

            return vulnerableReleases;
        }
    }
}
