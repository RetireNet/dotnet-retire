using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotNet.Retire.Runtimes.Core.Clients.Models
{
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
}
