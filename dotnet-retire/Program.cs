using System;
using System.Linq;

namespace dotnet_retire
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            var packagesToRetire = RetireService.GetPackagesToRetire();

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
