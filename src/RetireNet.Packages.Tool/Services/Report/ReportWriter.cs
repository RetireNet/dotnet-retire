using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RetireNet.Packages.Tool.Models.Report;

namespace RetireNet.Packages.Tool.Services.Report
{
    public class ReportWriter
    {
        private readonly ILogger<ReportWriter> _logger;
        private readonly string _basePath;
        private readonly string _path;
        private readonly string _format;
        private readonly IEnumerable<IReportGenerator> _generators;

        public ReportWriter(IOptions<RetireServiceOptions> options, IEnumerable<IReportGenerator> generators, ILogger<ReportWriter> logger)
            : this(options.Value, generators, logger)
        {
        }

        public ReportWriter(RetireServiceOptions options, IEnumerable<IReportGenerator> generators, ILogger<ReportWriter> logger)
        {
            _generators = generators;
            _logger = logger;
            _basePath = options.Path;
            _path = options.ReportPath;

            var foundFormat = FindFormat(options.ReportFormat, true);

            if (foundFormat == null)
            {
                _logger.LogCritical($"Could not find a format matching '{options.ReportFormat}. Reporting will be skipped.");
                _logger.LogCritical($"Valid formats are: {string.Join(", ", GetFormats())}");
            }

            _format = foundFormat;
        }

        public IEnumerable<string> GetFormats()
        {
            return _generators.Select(g => g.Format);
        }

        public string FindFormat(string format, bool ignoreCase)
        {
            var comp = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return _generators.FirstOrDefault(g => g.Format.Equals(format, comp))?.Format;
        }

        public void WriteReport(Models.Report.Report report)
        {
            if (string.IsNullOrWhiteSpace(_path))
            {
                _logger.LogInformation("No report path given, skipping report generation.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_format))
            {
                _logger.LogInformation("No format, skipping report generation.");
                return;
            }

            var generator = _generators.FirstOrDefault(g => g.Format == _format);

            if (generator == null)
            {
                _logger.LogInformation($"No generator for format '{_format} was found.");
                return;
            }

            // convert to relative paths...
            report.ForEach(p => p.Path = p.Path.Replace(_basePath, ""));

            File.WriteAllText(_path, generator.GenerateReport(report));
            _logger.LogInformation($"Wrote {generator.Format} report to {_path}.");
        }
    }
}
