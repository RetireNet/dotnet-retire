using System.IO;
using Microsoft.Extensions.Options;
using RetireNet.Packages.Tool.Services;

namespace DotNetRetire.Tests
{
    public class MockFileService : FileService
    {
        private readonly string _prefix;

        public MockFileService(string prefix) : base(new NoOpLogger(), Options.Create(new RetireServiceOptions()))
        {
            _prefix = prefix;
        }

        public override string NameOfAssetsJsonFile()
        {
            return _prefix + "." + base.NameOfAssetsJsonFile();
        }

        public override string PathOfAssetsFile()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
