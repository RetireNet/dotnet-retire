using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RetireNet.Packages.Tool.Extensions;
using RetireNet.Packages.Tool.Models;

namespace RetireNet.Packages.Tool.Services
{
    public class RetireApiClient
    {
        private readonly Uri _rootUrl;
        private readonly ILogger<RetireApiClient> _logger;

        public RetireApiClient(ILogger<RetireApiClient> logger, IOptions<RetireServiceOptions> options)
        {
            _logger = logger;
            _rootUrl = options.Value.RootUrl;
        }

        public IEnumerable<PackagesResponse> GetPackagesToRetire()
        {
            _logger.LogDebug($"Fetching known vulnerable packages from {_rootUrl}".Blue());
            var retireJsonUrl = _rootUrl;
            var start = HttpService.Get<Start>(retireJsonUrl);

            var packagesToRetire = new List<PackagesResponse>();
            foreach (var link in start.Links)
            {
                var packagesResponse = HttpService.Get<PackagesResponse>(link);
                packagesToRetire.Add(packagesResponse);
            }
            return packagesToRetire;
        }
    }
}
