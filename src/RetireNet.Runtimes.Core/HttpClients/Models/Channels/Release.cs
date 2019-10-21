using System.Collections.Generic;
using Newtonsoft.Json;
using RetireNet.Runtimes.Core.Clients.Models;

namespace RetireNet.Runtimes.Core.HttpClients.Models.Channels
{
    internal class Release
    {
        public Release()
        {
            CVEs = new List<CVE>();
        }

        [JsonProperty("release-version")]
        public string ReleaseVersion { get; set; }

        [JsonProperty("cve-list")]
        public IEnumerable<CVE> CVEs { get; set; }

        public Runtime Runtime { get; set; }

        public bool Security { get; set; }
    }
}
