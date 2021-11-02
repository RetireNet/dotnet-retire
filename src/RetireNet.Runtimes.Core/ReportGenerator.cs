using RetireNet.Runtimes.Core.Clients;
using RetireNet.Runtimes.Core.Clients.Models;

namespace RetireNet.Runtimes.Core;

public class ReportGenerator
{
    private readonly ReleaseMetadataClient _client;

    public ReportGenerator() : this(new ReleaseMetadataClient())
    {
    }

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

        var securityRelease = channel.Releases.FirstOrDefault(r => r.Security);

        if (securityRelease == null)
        {
            return new Report
            {
                AppRuntimeDetails = appRunTimeDetails,
                IsVulnerable = false
            };
        }

        var secReleaseVersion = Version.Parse(securityRelease.RuntimeVersion);
        var appReleaseVersion = Version.Parse(appRunTimeDetails.AppRuntimeVersion);

        if (appReleaseVersion >= secReleaseVersion)
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
