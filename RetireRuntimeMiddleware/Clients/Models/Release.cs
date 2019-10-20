using System.Collections.Generic;

namespace RetireRuntimeMiddleware.Clients.Models
{
    internal class Release
    {
        public Release()
        {
            CVEs = new List<CVE>();
        }

        public string ReleaseVersion { get; set; }

        public IEnumerable<CVE> CVEs { get; set; }

        public string RuntimeVersion { get; set; }

        public bool Security { get; set; }
    }
}
