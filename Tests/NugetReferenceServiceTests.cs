using System.IO;
using System.Linq;
using dotnet_retire;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Tests
{
    public class AssetServiceTests
    {
        [Fact]
        public void GetDirectReferencesWithSingleDependency()
        {
            var jObject = GetTestAssetFile("SingleTarget");
            var references = NugetReferenceService.GetNugetReferences(jObject);

            Assert.Equal(1, references.Count());
            Assert.Equal("Libuv", references.First().Id);
            Assert.Equal("1.9.1", references.First().Version);
            Assert.Equal(1, references.First().Dependencies.Count);
            Assert.Equal("Microsoft.NETCore.Platforms", references.First().Dependencies.First().Id);
            Assert.Equal("1.0.1", references.First().Dependencies.First().Version);
        }

        [Fact]
        public void GetDirectReferenceWithMultipleDependencies()
        {
            var jObject = GetTestAssetFile("SingleTarget-MultipleDependencies");
            var references = NugetReferenceService.GetNugetReferences(jObject);

            Assert.Equal(1, references.Count());
            Assert.Equal("Newtonsoft.Json", references.First().Id);
            Assert.Equal("10.0.2", references.First().Version);
            Assert.Equal(6, references.First().Dependencies.Count);
            Assert.Equal("Microsoft.CSharp", references.First().Dependencies.First().Id);
            Assert.Equal("4.3.0", references.First().Dependencies.First().Version);

            Assert.Equal("System.Xml.XmlDocument", references.First().Dependencies.Last().Id);
            Assert.Equal("4.3.0", references.First().Dependencies.Last().Version);
        }



        private static JObject GetTestAssetFile(string prefix)
        {
            var directory = Directory.GetCurrentDirectory();
            var testJson = File.ReadAllText($@"{directory}\\{prefix}.project.assets.json");
            var jObject = JObject.Parse(testJson);
            return jObject;
        }
    }
}
