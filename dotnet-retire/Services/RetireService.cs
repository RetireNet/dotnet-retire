using System;
using System.Collections.Generic;

namespace dotnet_retire
{
    public class RetireService
    {
        public static IEnumerable<Package> GetPackagesToRetire()
        {
            var url = "https://raw.githubusercontent.com/RetireNet/Packages/master/index.json";
            var retireJsonUrl = new Uri(url);
            var start = HttpService.Get<Start>(retireJsonUrl);

            var packagesToRetire = new List<Package>();
            Console.WriteLine($"Fetching known vulnerable packages from {url}".Blue());
            foreach (var link in start.Links)
            {
                var packagesResponse = HttpService.Get<PackagesResponse>(link);
                foreach (var p in packagesResponse.Packages)
                {
                    Console.WriteLine($"Checking for {p.Id}/{p.Affected}".Orange());
                }
                packagesToRetire.AddRange(packagesResponse.Packages);
            }
            return packagesToRetire;
        }
    }
}