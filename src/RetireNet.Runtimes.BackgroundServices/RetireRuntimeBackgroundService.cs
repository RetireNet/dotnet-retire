using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RetireNet.Runtimes.Core;
using RetireNet.Runtimes.Core.Clients.Models;

namespace RetireNet.Runtimes.BackgroundServices
{
    public class RetireRuntimeBackgroundService : BackgroundService
    {
        private readonly RetireRuntimeBackgroundServiceOptions _options;
        private readonly ReportGenerator _reportGenerator;
        private readonly ILogger<RetireRuntimeBackgroundService> _logger;

        public RetireRuntimeBackgroundService(IOptions<RetireRuntimeBackgroundServiceOptions> options, ReportGenerator reportGenerator, ILogger<RetireRuntimeBackgroundService> logger)
        {
            _options = options.Value;
            _reportGenerator = reportGenerator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var optionsCheckInterval = _options.CheckInterval;
            var timespan = new TimeSpan(0, 0, 0, 0, optionsCheckInterval);
            _logger.LogDebug($"Runtime vulnerability check is starting. Check interval: {timespan.ToString()}");

            stoppingToken.Register(() => _logger.LogDebug("Vulnerability check is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("Running runtime vulnerability check");


                var report = await _reportGenerator.GetReport(AppRunTimeDetails.Build());
                if (report.IsVulnerable.HasValue && report.IsVulnerable.Value)
                {
                    _logger.LogWarning("Running on vulnerable runtime {VulnerableRuntime}. Security release {SecurityPatch}", report.AppRuntimeDetails.AppRuntimeVersion, report.SecurityRelease.RuntimeVersion);

                }
                await Task.Delay(optionsCheckInterval, stoppingToken);
            }

            _logger.LogDebug($"GracePeriod background task is stopping.");
        }
    }
}
