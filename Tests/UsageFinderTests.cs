using System.Collections.Generic;
using System.Linq;
using dotnet_retire;
using Xunit;

namespace Tests
{
    public class UsageFinderTests
    {
        [Fact]
        public void DirectDependencyIsUsage()
        {
            var assets = new List<NugetReference>
            {
                new NugetReference
                {
                    Id = "SomePackageId",
                    Version = "1.0.0"
                }
            };

            var packages = new List<Package>{ new Package
            {
                Id = "SomePackageId",
                Affected = "1.0.0"
            }};
            var usages = UsagesFinder.FindUsagesOf(assets, packages);
            Assert.Equal(1, usages.Count());
        }

        [Fact]
        public void TransientDependencyIsUsage()
        {
            var assets = new List<NugetReference>
            {
                new NugetReference
                {
                    Id = "SomeOtherId",
                    Version = "2.0.0",
                    Dependencies = new List<NugetReference>
                    {
                        new NugetReference
                        {
                            Id = "SomePackageId",
                            Version = "1.0.0"
                        }
                    }
                }
            };

            var packages = new List<Package>{ new Package
            {
                Id = "SomePackageId",
                Affected = "1.0.0"
            }};

            var usages = UsagesFinder.FindUsagesOf(assets, packages);
            Assert.Equal(1, usages.Count());
            Assert.True(usages.First() is TransientUsage);
        }
    }
}
