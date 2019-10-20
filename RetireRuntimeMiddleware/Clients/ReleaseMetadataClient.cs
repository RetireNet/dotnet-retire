using System.Linq;
using System.Threading.Tasks;
using RetireRuntimeMiddleware.Clients.Models;
using RetireRuntimeMiddleware.HttpClients;

namespace RetireRuntimeMiddleware.Clients
{
    internal class ReleaseMetadataClient
    {
        private readonly ReleaseMetadataHttpClient _httpClient;

        public ReleaseMetadataClient()
        {
            _httpClient = new ReleaseMetadataHttpClient();
        }

        public async Task<Channel> GetChannel(string appRuntimeVersion)
        {
            var allChannels = await _httpClient.GetAllChannelsAsync();

            foreach (var singleChannel in allChannels)
            {
                var isChannelContainingRuntimeRelease = singleChannel.Releases.Any(r => r.ReleaseVersion == appRuntimeVersion && r.Runtime.Version == appRuntimeVersion);
                if (isChannelContainingRuntimeRelease)
                {
                    var channel = new Channel();
                    channel.Releases.AddRange(singleChannel.Releases.Select(r =>
                    {
                        var release = new Release
                        {
                            ReleaseVersion = r.ReleaseVersion,
                            RuntimeVersion = r.Runtime?.Version
                        };

                        if (r.Security)
                        {
                            release.Security = r.Security;
                            release.CVEs = r.CVEs?.Select(c => new CVE
                            {
                                Id = c.Id,
                                Url = c.Url
                            });
                        }
                        return release;
                    }));
                    return channel;
                }
            }

            return null;
        }
    }
}
