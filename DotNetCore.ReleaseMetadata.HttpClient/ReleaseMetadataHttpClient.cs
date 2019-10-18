using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetCore.ReleaseMetadata.HttpClient.Models;
using Newtonsoft.Json;

namespace RetireRuntimeMiddleware
{
    public class ReleaseMetadataHttpClient
    {
        private HttpClient _httpClient;

        public ReleaseMetadataHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            _httpClient.BaseAddress = new Uri("https://dotnetcli.blob.core.windows.net");
        }

        public async Task<Vulns> GetVulnerableRuntimes()
        {
            var vulns = new Vulns();
            var res = await _httpClient.GetAsync("/dotnet/release-metadata/releases-index.json");
            var data = await res.Content.ReadAsStringAsync();
            var index = JsonConvert.DeserializeObject<ReleaseIndex>(data, new JsonSerializerSettings());

            foreach (var channel in index.Channels)
            {
                var releasesForChannelRes = await _httpClient.GetAsync(channel.ReleasesUrl);
                var releasesData = await releasesForChannelRes.Content.ReadAsStringAsync();
                var releasesContainer = JsonConvert.DeserializeObject<ReleasesContainer>(releasesData);
                foreach (var release in releasesContainer.Releases)
                {
                    if (release.CVEs == null)
                        continue;

                    if (release.CVEs.Any())
                    {
                        vulns.Vulnerables.Add(release);
                    }
                }
            }

            return vulns;
        }
    }
}
