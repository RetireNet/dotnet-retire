using System;
using System.Collections.Generic;

namespace dotnet_retire
{
    public class RetireService
    {
        public static IEnumerable<Package> GetPackagesToRetire(string rootUrl)
        {
            var retireJsonUrl = new Uri(rootUrl);
            var start = HttpService.Get<Start>(retireJsonUrl);

            var packagesToRetire = new List<Package>();
            Console.WriteLine($"Fetching known vulnerable packages from {rootUrl}".Blue());
            foreach (var link in start.Links)
            {
                var packagesResponse = HttpService.Get<PackagesResponse>(link);
                packagesToRetire.AddRange(packagesResponse.Packages);
            }
            return packagesToRetire;
        }
    }
}