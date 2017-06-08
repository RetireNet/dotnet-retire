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
            var packagesToRetire = _retireApiClient.GetPackagesToRetire();
            foreach (var p in packagesToRetire)
            {
                _logger.LogDebug($"Looking for {p.Id}/{p.Affected}".Orange());
            }

            IEnumerable<NugetReference> nugetReferences = new List<NugetReference>();
            try
            {
                nugetReferences = _nugetreferenceservice.GetNugetReferences();
            }
            catch (NoAssetsFoundException)
            {
                _logger.LogWarning($"No assets found. Could not check dependencies. Missing 'dotnet restore'?");
                Environment.Exit(0);
                return;
            }

            _logger.LogInformation($"Found in total {nugetReferences.Count()} references of NuGets (direct & transient)");

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
        }
    }
}