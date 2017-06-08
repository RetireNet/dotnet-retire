using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dotnet_retire
{
    public class RetireApiClient
    {
        private readonly string _rootUrl;
        private readonly ILogger<RetireApiClient> _logger;

        public RetireApiClient(ILoggerFactory loggerFactory, IOptions<RetireServiceOptions> options)
        {
            _logger = loggerFactory.CreateLogger<RetireApiClient>();
            _rootUrl = options.Value.RootUrl;
        }

        public IEnumerable<Package> GetPackagesToRetire()
        {
            _logger.LogInformation($"Fetching known vulnerable packages from {_rootUrl}".Blue());
            var retireJsonUrl = new Uri(_rootUrl);
            var start = HttpService.Get<Start>(retireJsonUrl);

            var packagesToRetire = new List<Package>();
            foreach (var link in start.Links)
            {
                var packagesResponse = HttpService.Get<PackagesResponse>(link);
                packagesToRetire.AddRange(packagesResponse.Packages);
            }
            return packagesToRetire;
        }
    }
}