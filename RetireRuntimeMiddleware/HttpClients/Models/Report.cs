using RetireRuntimeMiddleware.HttpClients.Models;

namespace RetireRuntimeMiddleware.Middlewares.Models
{
    internal class Report
    {
        public bool vulnerable { get; set; }
        public AppRunTimeDetails AppRuntimeDetails { get; set; }
        public Release VulnerableRuntimeInfo { get; set; }
    }
}
