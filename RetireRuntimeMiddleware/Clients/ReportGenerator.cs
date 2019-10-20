using System.Linq;
using System.Threading.Tasks;
using RetireRuntimeMiddleware.Clients.Models;

namespace RetireRuntimeMiddleware.Clients
{
    internal class ReportGenerator
    {
        private readonly ReleaseMetadataClient _client;

        public ReportGenerator()
        {
            _client = new ReleaseMetadataClient();
        }

        public async Task<Report> GetReport()
        {
            var appRunTimeDetails = AppRunTimeDetails.New();
            var channel = await _client.GetChannel(appRunTimeDetails.AppRuntimeVersion);

            var securityRelease = channel.Releases.FirstOrDefault(r => r.Security);

            if (securityRelease == null || securityRelease.RuntimeVersion == appRunTimeDetails.AppRuntimeVersion)
            {
                return new Report
                {
                    AppRuntimeDetails = appRunTimeDetails,
                    IsVulnerable = false
                };
            }

            return new Report
            {
                IsVulnerable = true,
                AppRuntimeDetails = appRunTimeDetails,
                SecurityRelease = securityRelease
            };
        }


    }
}
