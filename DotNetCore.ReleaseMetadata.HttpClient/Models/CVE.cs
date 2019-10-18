using Newtonsoft.Json;

namespace DotNetCore.ReleaseMetadata.HttpClient.Models
{
    public class CVE
    {
        [JsonProperty("cve-id")]
        public string Id { get; set; }

        [JsonProperty("cve-url")]
        public string Url { get; set; }
    }
}