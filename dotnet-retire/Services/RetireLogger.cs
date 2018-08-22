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
        private readonly AssetsFileParser _nugetreferenceservice;
        private readonly UsagesFinder _usageFinder;

        public RetireLogger(ILogger<RetireLogger> logger, RetireApiClient retireApiClient, AssetsFileParser nugetreferenceservice, UsagesFinder usageFinder)
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

            var packagesToRetire = _retireApiClient.GetPackagesToRetire();
            foreach (var p in packagesToRetire)
            {
                _logger.LogTrace($"Looking for {p.Id}/{p.Affected}".Orange());
            }

            IEnumerable<NugetReference> nugetReferences = new List<NugetReference>();
            try
            {
                nugetReferences = _nugetreferenceservice.GetNugetReferences();
            }
            catch (NoAssetsFoundException)
            {
                _logger.LogError("No assets found. Could not check dependencies. Missing 'dotnet restore' or are you running the tool from a folder missing a csproj?");
                return;
            }

            _logger.LogDebug($"Found in total {nugetReferences.Count()} references of NuGets (direct & transient)");

            var usages = _usageFinder.FindUsagesOf(nugetReferences, packagesToRetire);

            if (usages.Any())
            {
                foreach (var usage in usages)
                {
                    _logger.LogError($"Found direct reference to {usage.NugetReference}".Red());
                }
            }
            else
            {
                _logger.LogInformation($"Found no usages of vulnerable libs!".Green());
            }

            _logger.LogInformation($"Scan complete.");

        }
    }
}
