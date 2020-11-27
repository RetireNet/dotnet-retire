
using System.Text.Json.Serialization;

namespace RetireNet.Runtimes.Core.Clients.Models
{
    public class CVE
    {
        [JsonPropertyName("cve-id")]
        public string Id { get; set; }

        [JsonPropertyName("cve-url")]
        public string Url { get; set; }
    }
}
