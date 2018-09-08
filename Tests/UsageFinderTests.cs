using System.Collections.Generic;
using System.Linq;
using dotnet_retire;
using Xunit;

namespace Tests
{
    public class UsageFinderTests
    {
        public UsagesFinder UsagesFinder => new UsagesFinder();

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
            Assert.Single(usages);
        }

        [Fact]
        public void TransientDependencyWhenOverriddenWithDirectReferenceIsNotVulnerableUsage()
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
                },
                new NugetReference
                {
                    Id = "SomePackageId",
                    Version = "2.0.0"
                }
            };

            var packages = new List<Package>{ new Package
            {
                Id = "SomePackageId",
                Affected = "1.0.0"
            }};

            var usages = UsagesFinder.FindUsagesOf(assets, packages);
            Assert.Empty(usages);
        }

        [Fact]
        public void MultipleDependencyPathsFindsMultipleUsages()
        {
            var assets = new List<NugetReference>
            {
                new NugetReference
                {
                    Id = "A",
                    Version = "2.0.0",
                    Dependencies = new List<NugetReference>
                    {
                        new NugetReference
                        {
                            Id = "Vulnerable",
                            Version = "2.0.0"
                        }
                    }
                },
                new NugetReference
                {
                    Id = "B",
                    Version = "3.0.0",
                    Dependencies = new List<NugetReference>
                    {
                        new NugetReference
                        {
                            Id = "Vulnerable",
                            Version = "2.0.0"
                        }
                    }
                },
                new NugetReference
                {
                    Id = "Vulnerable",
                    Version = "2.0.0"
                }
            };

            var packages = new List<Package>{ new Package
            {
                Id = "Vulnerable",
                Affected = "2.0.0"
            }};

            var usages = UsagesFinder.FindUsagesOf(assets, packages);
            Assert.Equal(2, usages.Count);
            Assert.Equal(1, usages.Count(u => u.OuterMostPackage.Id == "A"));
            Assert.Equal(1, usages.Count(u => u.OuterMostPackage.Version == "2.0.0"));

            Assert.Equal(1, usages.Count(u => u.OuterMostPackage.Id == "B"));
            Assert.Equal(1, usages.Count(u => u.OuterMostPackage.Version == "3.0.0"));
        }

        [Fact]
        public void VulnerableDependencyMultipleDependenciesDown()
        {
            var assets = new List<NugetReference>
            {
                new NugetReference
                {
                    Id = "A",
                    Version = "2.0.0",
                    Dependencies = new List<NugetReference>
                    {
                        new NugetReference
                        {
                            Id = "B",
                            Version = "2.0.0"
                        }
                    }
                },
                new NugetReference
                {
                    Id = "B",
                    Version = "2.0.0",
                    Dependencies = new List<NugetReference>
                    {
                        new NugetReference
                        {
                            Id = "Vulnerable",
                            Version = "2.0.0"
                        }
                    }
                },
                new NugetReference
                {
                    Id = "Vulnerable",
                    Version = "2.0.0"
                }
            };

            var packages = new List<Package>{ new Package
            {
                Id = "Vulnerable",
                Affected = "2.0.0"
            }};

            var usages = UsagesFinder.FindUsagesOf(assets, packages);
            Assert.Single(usages);
            var usage = usages.Single();
            Assert.Equal("A", usage.OuterMostPackage.Id);
            Assert.Equal("2.0.0", usage.OuterMostPackage.Version);
            Assert.True(usage.ReadPath().Contains("A") && usage.ReadPath().Contains("B"));
        }
    }
}
