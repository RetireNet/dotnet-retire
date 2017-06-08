using System.Collections.Generic;
using System.Linq;

namespace dotnet_retire
{
    public class UsagesFinder
    {
        public IEnumerable<Usage> FindUsagesOf(IEnumerable<NugetReference> assets, IEnumerable<Package> knownVulnerables)
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
            }

            return usages;
        }
    }
}
