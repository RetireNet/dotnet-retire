using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnet_retire
{
    public class Program
    {
        static void Main(string[] args)
        {
            var rootApiUrl = "https://raw.githubusercontent.com/RetireNet/Packages/master/index.json";
            Console.WriteLine($"Fetching known vulnerable packages from {rootApiUrl}".Blue());
            var packagesToRetire = RetireService.GetPackagesToRetire(rootApiUrl);
            foreach (var p in packagesToRetire)
            {
                Console.WriteLine($"Looking for {p.Id}/{p.Affected}".Orange());
            }

            IEnumerable<NugetReference> nugetReferences = new List<NugetReference>();
            new List<NugetReference>();
            try
            {
                 nugetReferences = NugetReferenceService.GetNugetReferences();
            }
            catch (NoAssetsFoundException)
            {
                Console.WriteLine($"No assets found. Could not check dependencies. Missing 'dotnet restore'?");
                Environment.Exit(0);
                return;
            }

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
