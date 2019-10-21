namespace DotNet.Retire.Runtimes.Core.Clients.Models
{
    public class Report
    {
        public bool? IsVulnerable { get; set; }
        public string Details { get; set; }
        public AppRunTimeDetails AppRuntimeDetails { get; set; }
        public Release SecurityRelease { get; set; }
    }
}
