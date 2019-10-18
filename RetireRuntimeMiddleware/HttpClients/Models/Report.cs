using System.Collections.Generic;
using RetireRuntimeMiddleware.HttpClients.Models;

namespace RetireRuntimeMiddleware.Middlewares.Models
{
    internal class Report
    {
        public bool IsVulnerable { get; set; }
        public AppRunTimeDetails AppRuntimeDetails { get; set; }

        public ReleaseInfo VulnerableRelease { get; set; }
        public ReleaseInfo SecurityRelease { get; set; }
    }

    internal class ReleaseInfo
    {
        public string RuntimeVersion { get; set; }

        public IEnumerable<CVE> CVEs { get; set; }
    }
}
