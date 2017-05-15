using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnet_retire
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            var url = "https://raw.githubusercontent.com/RetireNet/Packages/master/index.json";
            var retireJsonUrl = new Uri(url);
            var start = HttpService.Get<Start>(retireJsonUrl);

            var allPackages = new List<Package>();
            Console.WriteLine($"Fetching known vulnerable packages from {url}".Blue());
            foreach (var link in start.Links)
            {
                var packagesResponse = HttpService.Get<PackagesResponse>(link);
                foreach (var p in packagesResponse.Packages)
                {
                    Console.WriteLine($"Checking for {p.Id}/{p.Affected}".Orange());
                }
                allPackages.AddRange(packagesResponse.Packages);
            }

            var projectAssets = AssetsService.GetAssets();
            Console.WriteLine($"Found {projectAssets.Assets.Count} assets");

            var usages = UsagesFinder.FindUsagesOf(projectAssets.Assets, allPackages);

            if (usages.Any())
            {
                foreach (var usage in usages)
                {
                    if (usage is TransientUsage)
                    {
                        var tUsage = usage as TransientUsage;
                        Console.WriteLine($"Found transient usage of {usage.Asset} via {tUsage.ParentAsset}".Red());
                    }
                    else
                    {
                        Console.WriteLine($"Found direct usage of {usage.Asset}".Red());
                    }

                }
            }
            else
            {
                Console.WriteLine($"Found no usages of vulnerable libs!".Green());
            }
        }
    }
}
