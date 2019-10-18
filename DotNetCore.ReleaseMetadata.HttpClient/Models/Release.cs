using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotNetCore.ReleaseMetadata.HttpClient.Models
{
    public class Release
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
    }
}