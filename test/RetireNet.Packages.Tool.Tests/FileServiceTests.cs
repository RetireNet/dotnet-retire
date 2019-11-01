using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RetireNet.Packages.Tool.Services;
using System.IO;
using Xunit;

namespace DotNetRetire.Tests
{
    public class FileServiceTests
    {
        private static string TransformPath(string path) =>
            path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("FileX")]
        [InlineData("FileX.ext")]
        public void GetDirectoryAndFileWithInvalidPaths_Exception(string path)
        {
            var fileService = new FileService(NullLogger<FileService>.Instance, Options.Create(new RetireServiceOptions { Path = path }));

            Assert.Throws<DirectoryNotFoundException>(() => fileService.GetDirectory());
            Assert.Throws<FileNotFoundException>(() => fileService.GetFile());
        }

        [Theory]
        [InlineData("TestFiles/SingleTarget")]
        [InlineData("TestFiles\\SingleTarget")]
        public void GetDirectoryAndFileWithValidPaths_Success(string path)
        {
            var fileService = new FileService(NullLogger<FileService>.Instance, Options.Create(new RetireServiceOptions { Path = path }));

            Assert.EndsWith(TransformPath("TestFiles/SingleTarget"), fileService.GetDirectory());
            Assert.EndsWith(TransformPath("TestFiles/SingleTarget/obj/project.assets.json"), fileService.GetFile());
        }
    }
}
