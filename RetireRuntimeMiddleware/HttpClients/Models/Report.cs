using System.Collections.Generic;
using RetireRuntimeMiddleware.HttpClients.Models;

namespace RetireRuntimeMiddleware.Middlewares.Models
{
    internal class Report
    {
        public bool IsVulnerable { get; set; }
        public AppRunTimeDetails AppRuntimeDetails { get; set; }

        public Release VulnerableRelease { get; set; }
        public Release SecurityRelease { get; set; }
    }
}
