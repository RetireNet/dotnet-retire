using System.IO;
using System.Linq;
using dotnet_retire;
using Xunit;

namespace Tests
{
    public class AssetServiceTests
    {
        public AssetsFileParser NugetReferenceService(string prefix)
        {
            var mock = new MockFileService();
            mock.SetAssetFile(prefix);
            return new AssetsFileParser(mock);
        }

        [Fact]
        public void GetDirectReferencesWithSingleDependency()
        {
            var references = NugetReferenceService("SingleTarget").GetNugetReferences();

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
            var references = NugetReferenceService("SingleTarget-MultipleDependencies").GetNugetReferences();

            Assert.Equal(1, references.Count());
            Assert.Equal("Newtonsoft.Json", references.First().Id);
            Assert.Equal("10.0.2", references.First().Version);
            Assert.Equal(6, references.First().Dependencies.Count);
            Assert.Equal("Microsoft.CSharp", references.First().Dependencies.First().Id);
            Assert.Equal("4.3.0", references.First().Dependencies.First().Version);

            Assert.Equal("System.Xml.XmlDocument", references.First().Dependencies.Last().Id);
            Assert.Equal("4.3.0", references.First().Dependencies.Last().Version);
        }

    }

    public class MockFileService : IFileService
    {
        public string GetProjectAssetsJsonObject()
        {
            return FileContents;
        }

        public string FileContents { get; set; }

        public void SetAssetFile(string prefix)
        {
            var directory = Directory.GetCurrentDirectory();
            FileContents = File.ReadAllText($@"{directory}\\{prefix}.project.assets.json");
        }

        public string GetFileContents()
        {
            return FileContents;
        }
    }
}
