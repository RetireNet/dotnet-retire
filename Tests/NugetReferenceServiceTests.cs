using System.Linq;
using dotnet_retire;
using NuGet.ProjectModel;
using Xunit;

namespace Tests
{
    public class AssetServiceTests
    {
        public NugetProjectModelAssetsFileParser NugetReferenceService(string prefix)
        {
            var mock = new MockFileService(prefix);

            return new NugetProjectModelAssetsFileParser(mock);
        }

        [Fact]
        public void GetDirectReferencesWithSingleDependency()
        {
            var references = NugetReferenceService("SingleTarget").GetNugetReferences();

            Assert.Single(references);
            Assert.Equal("Libuv", references.First().Id);
            Assert.Equal("1.9.1", references.First().Version);
            Assert.Single(references.First().Dependencies);
            Assert.Equal("Microsoft.NETCore.Platforms", references.First().Dependencies.First().Id);
            Assert.Equal("1.0.1", references.First().Dependencies.First().Version);
        }

        [Fact]
        public void GetDirectReferenceWithMultipleDependencies()
        {
            var references = NugetReferenceService("SingleTarget-MultipleDependencies").GetNugetReferences();

            Assert.Single(references);
            Assert.Equal("Newtonsoft.Json", references.First().Id);
            Assert.Equal("10.0.2", references.First().Version);
            Assert.Equal(6, references.First().Dependencies.Count);
            Assert.Equal("Microsoft.CSharp", references.First().Dependencies.First().Id);
            Assert.Equal("4.3.0", references.First().Dependencies.First().Version);

            Assert.Equal("System.Xml.XmlDocument", references.First().Dependencies.Last().Id);
            Assert.Equal("4.3.0", references.First().Dependencies.Last().Version);
        }

        [Fact]
        public void GetVulnerableDapperButWithManuallyUpdateSystemNetSecurity()
        {
            var references = NugetReferenceService("SingleTarget.ManuallyFixedUpgrade").GetNugetReferences().ToArray();

            Assert.Equal(3, references.Count());

            Assert.Equal("Dapper", references[0].Id);
            Assert.Equal("1.50.2", references[0].Version);
            Assert.Equal(18, references[0].Dependencies.Count);

            Assert.Equal("System.Data.SqlClient", references[1].Id);
            Assert.Equal("4.1.0", references[1].Version);

            Assert.Equal("System.Net.Security", references[2].Id);
            Assert.Equal("4.3.1", references[2].Version);
        }
    }
}
