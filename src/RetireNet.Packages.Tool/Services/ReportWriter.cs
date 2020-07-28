using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RetireNet.Packages.Tool.Models.Report;

namespace RetireNet.Packages.Tool.Services
{
    public class ReportWriter
    {
        private readonly ILogger<ReportWriter> _logger;
        private readonly string _basePath;
        private readonly string _path;
        private readonly Format _format;

        public ReportWriter(IOptions<RetireServiceOptions> options, ILogger<ReportWriter> logger) : this(options.Value, logger)
        {
        }

        public ReportWriter(RetireServiceOptions options, ILogger<ReportWriter> logger)
        {
            _logger = logger;
            _path = options.ReportPath;
            _basePath = options.Path;
            _format = options.ReportFormat;
        }

        public void WriteReport(Report report)
        {
            if (string.IsNullOrWhiteSpace(_path))
            {
                _logger.LogInformation("No report path given, skipping report generation.");
                return;
            }

            report.ForEach(p => p.Path = p.Path.Replace(_basePath, ""));

            if (_format == Format.Json)
            {
                File.WriteAllText(_path, JsonConvert.SerializeObject(report));
                _logger.LogInformation($"Wrote JSON report to {_path}.");
                return;
            }

            var markdown = new StringBuilder();
            markdown.AppendLine("# Retire Vulnerability Report");
            markdown.AppendLine();
            foreach (var project in report)
            {
                markdown.AppendLine($"## {project.Name} ({project.Path})");
                markdown.AppendLine();

                var groups =
                    project.Issues.GroupBy(i => $"{i.ProblemPackage.Name} (v{i.ProblemPackage.Version})");

                foreach (var packageGroup in groups)
                {
                    markdown.AppendLine($"### {packageGroup.Key}");
                    foreach (var issue in packageGroup)
                    {
                        var description = $"[{issue.Description}]({issue.IssueUrl})";
                        markdown.AppendLine($" * {description}");

                        foreach (var packageChain in issue.PackageChains)
                        {
                            if (packageChain.Count() == 1)
                            {
                                continue;
                            }

                            for (var i = 0; i < packageChain.Count(); i++)
                            {
                                var chainItem = packageChain[i];
                                var str = $"* {chainItem.Name}/{chainItem.Version}";

                                markdown.AppendLine(str.PadLeft(str.Length + (2 * i) + 3, ' '));
                            }
                        }
                    }
                }
            }

            File.WriteAllText(_path, markdown.ToString());
            _logger.LogInformation($"Wrote MarkDown report to {_path}.");
        }
    }
}
