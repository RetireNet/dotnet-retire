using Newtonsoft.Json;
using RetireNet.Packages.Tool.Models.Reporting;

namespace RetireNet.Packages.Tool.Services.Reporting.Generators
{
    public class JsonReportGenerator : IReportGenerator
    {
        public string Format => "JSON";
        public string GenerateReport(Report report)
        {
            return JsonConvert.SerializeObject(report, Formatting.Indented);
        }
    }
}
