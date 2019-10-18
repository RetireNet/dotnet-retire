using Newtonsoft.Json;

namespace RetireRuntimeMiddleware.HttpClients.Models
{
    internal class CVE
    {
        [JsonProperty("cve-id")]
        public string Id { get; set; }

        [JsonProperty("cve-url")]
        public string Url { get; set; }
    }
}
