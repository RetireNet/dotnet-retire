using System.Collections.Generic;
using System.Linq;

namespace dotnet_retire
{
    public class UsagesFinder
    {
        public List<Usage> FindUsagesOf(List<NugetReference> assets, List<Package> knownVulnerables)
        {
            var usages = new List<Usage>();

            foreach (var asset in assets)
            {
                var directPackage = knownVulnerables.FirstOrDefault(k => k.Id == asset.Id && k.Affected == asset.Version);

                if (directPackage != null)
                {
                    var usage = new Usage
                    {
                        Package = directPackage
                    };
                    usage.Add(asset);
                    var differentUsages = FindPathsTo(usage, assets);
                    usages.AddRange(differentUsages);
                }
            }

            return usages;
        }

        private static IEnumerable<Usage> FindPathsTo(Usage usage, List<NugetReference> assets)
        {
            var allWithThisChild = assets.Where(d => d.Dependencies.Any(c => c.Id == usage.OuterMostId)).ToList();
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
