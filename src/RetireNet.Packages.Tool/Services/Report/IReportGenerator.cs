namespace RetireNet.Packages.Tool.Services.Report
{
    public interface IReportGenerator
    {
        string Format { get; }
        string GenerateReport(Models.Report.Report report);
    }
}
