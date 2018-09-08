using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace dotnet_retire
{
    public class RetireLogger
    {
        private readonly ILogger<RetireLogger> _logger;
        private readonly RetireApiClient _retireApiClient;
        private readonly IAssetsFileParser _nugetreferenceservice;
        private readonly UsagesFinder _usageFinder;
        private readonly DotNetRestorer _restorer;

        public RetireLogger(ILogger<RetireLogger> logger, RetireApiClient retireApiClient, IAssetsFileParser nugetreferenceservice, UsagesFinder usageFinder, DotNetRestorer restorer)
        {
            _logger = logger;
            _retireApiClient = retireApiClient;
            _nugetreferenceservice = nugetreferenceservice;
            _usageFinder = usageFinder;
            _restorer = restorer;
        }

        public void LogPackagesToRetire()
        {
            // removing this line breaks logging somehow.
            _logger.LogInformation("Scan starting".Green());

            var packagesToRetire = _retireApiClient.GetPackagesToRetire().ToList();
            foreach (var p in packagesToRetire)
            {
                _logger.LogTrace($"Looking for {p.Id}/{p.Affected}".Orange());
            }

            var status = _restorer.Restore();
            if (status.IsSuccess)
            {
                _logger.LogDebug("`dotnet restore:`" + status.Output);
            }
            else
            {
                _logger.LogDebug("`dotnet restore output:`" + status.Output);
                _logger.LogDebug("`dotnet restore errors:`" + status.Errors);
                _logger.LogDebug("`dotnet restore exitcode:`" + status.ExitCode);

                _logger.LogError("Failed to `dotnet restore`. Is the current dir missing a csproj?");
                return;
            }

            var nugetReferences = _nugetreferenceservice.GetNugetReferences().ToList();

            _logger.LogDebug($"Found in total {nugetReferences.Count} references of NuGets (direct & transient)");

            var usages = _usageFinder.FindUsagesOf(nugetReferences, packagesToRetire);

            if (usages.Any())
            {
                var plural = usages.Count > 1 ? "s" : "";
                var grouped = usages.GroupBy(g => g.NugetReference.ToString());
                var errorLog = $"Found use of {grouped.Count()} vulnerable libs in {usages.Count} dependency path{plural}.";

                foreach (var group in grouped)
                {
                    errorLog += $"\n\n* {group.Key}".Red();

                    foreach (var usage in group)
                    {
                        if(!usage.IsDirect)
                            errorLog += $"\n{usage.ReadPath()}";
                    }
                }

                errorLog += "\n";
                _logger.LogError(errorLog);
            }
            else
            {
                _logger.LogInformation($"Found no usages of vulnerable libs!".Green());
            }

            _logger.LogInformation($"Scan complete.");

        }
    }
}
