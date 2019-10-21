using System.Linq;
using System.Threading.Tasks;
using DotNet.Retire.Runtimes.Core.Clients.Models;
using DotNet.Retire.Runtimes.Core.HttpClients;

namespace DotNet.Retire.Runtimes.Core.Clients
{
    public class ReleaseMetadataClient
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
                var isChannelContainingRuntimeRelease = singleChannel.Releases.Any(r => r.Runtime != null && r.Runtime.Version == appRuntimeVersion);
                if (isChannelContainingRuntimeRelease)
                {
                    var channel = new Channel();
                    channel.Releases.AddRange(singleChannel.Releases.Select(r =>
                    {
                        var release = new Release
                        {
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
