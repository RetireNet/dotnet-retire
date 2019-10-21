using System.Collections.Generic;
using System.Linq;
using DotNet.Retire.Packages.Tool.Models;

namespace DotNet.Retire.Packages.Tool.Services
{
    public class UsagesFinder
    {
        public List<Usage> FindUsagesOf(List<NugetReference> assets, List<PackagesResponse> knownVulnerables)
        {
            var usages = new List<Usage>();

            foreach (var asset in assets)
            {
                foreach(var vulnPackage in knownVulnerables)
                {
                    var directPackage = vulnPackage.Packages.FirstOrDefault(k => k.Id == asset.Id && k.Affected == asset.Version);

                    if (directPackage != null)
                    {
                        var usage = new Usage
                        {
                            Package = directPackage,
                            IssueUrl = vulnPackage.Link,
                            Description = vulnPackage.Description
                        };
                        usage.Add(asset);
                        var differentUsages = FindPathsTo(usage, assets);
                        usages.AddRange(differentUsages);
                    }
                }
            }

            return usages;
        }

        private static IEnumerable<Usage> FindPathsTo(Usage usage, List<NugetReference> assets)
        {
            var allWithThisChild = assets.Where(d => d.Dependencies.Any(c => c.Id == usage.OuterMostPackage.Id && c.Version == usage.OuterMostPackage.Version)).ToList();
            if (allWithThisChild.Count == 0)
                yield return usage;
            else
            {
                foreach (var withThisChild in allWithThisChild)
                {
                    var copy = usage.Copy();
                    copy.Add(withThisChild);
                    foreach (var usagePath in FindPathsTo(copy, assets))
                    {
                        yield return usagePath;
                    }
                }
            }
        }
    }
}
