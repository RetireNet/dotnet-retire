using System.Linq;
using System.Threading.Tasks;
using RetireRuntimeMiddleware.Clients.Models;

namespace RetireRuntimeMiddleware.Clients
{
    internal class ReportGenerator
    {
        private readonly ReleaseMetadataClient _client;

        public ReportGenerator(ReleaseMetadataClient client)
        {
            _client = client;
        }

        public async Task<Report> GetReport(AppRunTimeDetails appRunTimeDetails)
        {
            var channel = await _client.GetChannel(appRunTimeDetails.AppRuntimeVersion);

            if (channel == null)
            {
                return new Report
                {
                    AppRuntimeDetails = appRunTimeDetails,
                    Details = $"Running on unknown runtime {appRunTimeDetails.AppRuntimeVersion}. Not able to check for security patches."
                };
            }

            var securityRelease = channel?.Releases.FirstOrDefault(r => r.Security);

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
