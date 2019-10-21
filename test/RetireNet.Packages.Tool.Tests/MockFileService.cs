using System.IO;
using RetireNet.Packages.Tool.Services;

namespace DotNetRetire.Tests
{
    public class MockFileService : FileService
    {
        private readonly string _prefix;

        public MockFileService(string prefix) : base(new NoOpLogger())
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
