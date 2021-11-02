using System.Text.Json.Serialization;

namespace RetireNet.Runtimes.Core.Clients.Models;

public class Release
{
    public Release()
    {
        CVEs = new List<CVE>();
    }
    public string RuntimeVersion { get; set; }
    public IEnumerable<CVE> CVEs { get; set; }

    [JsonIgnore]
    public bool Security { get; set; }
}