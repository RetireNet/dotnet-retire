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

        public RetireLogger(ILogger<RetireLogger> logger, RetireApiClient retireApiClient, IAssetsFileParser nugetreferenceservice, UsagesFinder usageFinder)
        {
            _logger = logger;
            _retireApiClient = retireApiClient;
            _nugetreferenceservice = nugetreferenceservice;
            _usageFinder = usageFinder;
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

            List<NugetReference> nugetReferences;
            try
            {
                nugetReferences = _nugetreferenceservice.GetNugetReferences().ToList();
            }
            catch (NoAssetsFoundException)
            {
                _logger.LogError("No assets found. Could not check dependencies. Missing 'dotnet restore' or are you running the tool from a folder missing a csproj?");
                return;
            }

            _logger.LogDebug($"Found in total {nugetReferences.Count} references of NuGets (direct & transient)");

            var usages = _usageFinder.FindUsagesOf(nugetReferences, packagesToRetire);

            if (usages.Any())
            {
                var plural = usages.Count > 1 ? "s" : "";
                var errorLog = $"Found usage of vulnerable libs in {usages.Count} dependency path{plural}.";
                foreach (var usage in usages)
                {
                    errorLog +=$"\n* {usage.NugetReference}".Red() + $"{(usage.IsDirect ? "\n" : $"\n{usage.ReadPath()}")}";
                }

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
