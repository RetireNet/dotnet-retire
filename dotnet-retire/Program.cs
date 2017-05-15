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

            var jObject = FileService.GetProjectAssetsJsonObject();
            var nugetReferences = NugetReferenceService.GetNugetReferences(jObject);
            Console.WriteLine($"Found in total {nugetReferences.Count()} references of NuGets (direct & transient)");

            var usages = UsagesFinder.FindUsagesOf(nugetReferences, packagesToRetire);

            if (usages.Any())
            {
                foreach (var usage in usages)
                {
                    if (usage is TransientUsage)
                    {
                        var tUsage = usage as TransientUsage;
                        Console.WriteLine($"Found transient reference of {usage.NugetReference} via {tUsage.ParentNugetReference}".Red());
                    }
                    else
                    {
                        Console.WriteLine($"Found direct reference to {usage.NugetReference}".Red());
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
