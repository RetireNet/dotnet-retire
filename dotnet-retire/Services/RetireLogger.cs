using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace dotnet_retire
{
    public class RetireLogger
    {
        private readonly ILogger _logger;
        private readonly RetireApiClient _retireApiClient;

        public RetireLogger(ILoggerFactory loggerFactory, RetireApiClient retireApiClient)
        {
            _logger = loggerFactory.CreateLogger<RetireLogger>();
            _retireApiClient = retireApiClient;
        }

        public void LogPackagesToRetire()
        {
            var packagesToRetire = _retireApiClient.GetPackagesToRetire();
            foreach (var p in packagesToRetire)
            {
                LoggerExtensions.LogDebug(_logger, $"Looking for {p.Id}/{p.Affected}".Orange());
            }

            IEnumerable<NugetReference> nugetReferences = new List<NugetReference>();
            try
            {
                nugetReferences = NugetReferenceService.GetNugetReferences();
            }
            catch (NoAssetsFoundException)
            {
                LoggerExtensions.LogWarning(_logger, $"No assets found. Could not check dependencies. Missing 'dotnet restore'?");
                Environment.Exit(0);
                return;
            }

            LoggerExtensions.LogInformation(_logger, $"Found in total {nugetReferences.Count()} references of NuGets (direct & transient)");

            var usages = UsagesFinder.FindUsagesOf(nugetReferences, packagesToRetire);

            if (usages.Any())
            {
                foreach (var usage in usages)
                {
                    if (usage is TransientUsage tUsage)
                    {
                        LoggerExtensions.LogInformation(_logger, $"Found transient reference of {usage.NugetReference} via {tUsage.ParentNugetReference}".Red());
                    }
                    else
                    {
                        LoggerExtensions.LogInformation(_logger, $"Found direct reference to {usage.NugetReference}".Red());
                    }
                }
            }
            else
            {
                LoggerExtensions.LogInformation(_logger, $"Found no usages of vulnerable libs!".Green());
            }
        }
    }
}