using RetireNet.Packages.Tool.Models.Reporting;

namespace RetireNet.Packages.Tool.Services.Reporting
{
    public interface IReportGenerator
    {
        string Format { get; }
        string GenerateReport(Report report);
    }
}
