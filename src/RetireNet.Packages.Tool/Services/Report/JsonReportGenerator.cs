using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace RetireNet.Packages.Tool.Services.Report
{
    public class JsonReportGenerator : IReportGenerator
    {
        public string Format => "JSON";
        public string GenerateReport(Models.Report.Report report)
        {
            return JsonConvert.SerializeObject(report);
        }
    }
}
