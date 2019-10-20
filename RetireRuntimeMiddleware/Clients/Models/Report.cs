namespace RetireRuntimeMiddleware.Clients.Models
{
    internal class Report
    {
        public bool IsVulnerable { get; set; }
        public AppRunTimeDetails AppRuntimeDetails { get; set; }
        public Release SecurityRelease { get; set; }
    }
}
