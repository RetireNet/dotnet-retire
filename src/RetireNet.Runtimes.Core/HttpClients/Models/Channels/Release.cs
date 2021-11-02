using System.Text.Json.Serialization;
using RetireNet.Runtimes.Core.Clients.Models;

namespace RetireNet.Runtimes.Core.HttpClients.Models.Channels;

internal class Release
{
    public Release()
    {
        CVEs = new List<CVE>();
    }

    [JsonPropertyName("release-version")]
    public string ReleaseVersion { get; set; }

    [JsonPropertyName("cve-list")]
    public List<CVE> CVEs { get; set; }

    public Runtime Runtime { get; set; }

    public bool Security { get; set; }
}