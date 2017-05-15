using System.Collections.Generic;
using System.Linq;

namespace dotnet_retire
{
    public static class UsagesFinder
    {
        public static IEnumerable<Usage> FindUsagesOf(IEnumerable<NugetReference> assets, IEnumerable<Package> knownVulnerables)
        {
            var usages = new List<Usage>();

            foreach (var asset in assets)
            {
                var directPackage = knownVulnerables.FirstOrDefault(k => k.Id == asset.Id && k.Affected == asset.Version);

                if (directPackage != null)
                {
                    usages.Add(new Usage
                    {
                        NugetReference = asset,
                        Package = directPackage
                    });
                }

                foreach (var transientAsset in asset.Dependencies)
                {
                    var transientPackage = knownVulnerables.FirstOrDefault(k => k.Id == transientAsset.Id && k.Affected == transientAsset.Version);
                    if (transientPackage != null)
                    {
                        usages.Add(new TransientUsage
                        {
                            ParentNugetReference = asset,
                            NugetReference = transientAsset,
                            Package = transientPackage
                        });
                    }
                }
            }

            return usages;
        }
    }
}
