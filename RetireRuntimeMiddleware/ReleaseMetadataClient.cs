using System.Collections;
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
            var channelReports = await GetVulnerableRuntimes();
            var channel = channelReports.FindChannel(appRunTimeDetails.RuntimeVersion);

            return new Report
            {
                AppRuntimeDetails = appRunTimeDetails,
                IsVulnerable = channel.VulnerableRelease != null,
                VulnerableRelease = channel.VulnerableRelease,
                SecurityRelease = channel.SecurityRelease,
            };
        }

        private async Task<VulnerabilityReport> GetVulnerableRuntimes()
        {
            var vulnerabilityReport = new VulnerabilityReport();
            var index = await _httpClient.GetIndexAsync();

            var tasks = new List<Task<ChannelReport>>();

            foreach (var channel in index.Channels)
            {
                tasks.Add(GetChannelReport(channel));
            }

            var allChannels = await Task.WhenAll(tasks);

            foreach (var channelReport in allChannels)
            {
                vulnerabilityReport.AddChannelReport(channelReport);
            }

            return vulnerabilityReport;
        }

        private async Task<ChannelReport> GetChannelReport(Channel channel)
        {
            var releasesContainer = await _httpClient.GetReleases(channel.ReleasesUrl);

            var vulnerableReleases = new List<Release>();

            var securityRelease = releasesContainer.Releases.FirstOrDefault(r => r.Security);
            if (securityRelease != null)
            {
                vulnerableReleases.AddRange(releasesContainer.Releases.Where(r => r.ReleaseVersion != securityRelease.ReleaseVersion));
            }

            return new ChannelReport
            {
                SecurityRelease = securityRelease,
                AllVulnerablesExceptLatestSecurityRelease = vulnerableReleases
            };
        }
    }

    internal class ChannelReport
    {
        public ChannelReport()
        {

        }
        public Release SecurityRelease { get; set; }
        public IEnumerable<Release> AllVulnerablesExceptLatestSecurityRelease { get; set; }
        public Release VulnerableRelease { get; set; }
    }
}
