using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RetireNet.Packages.Tool.Services;
using System.IO;
using Xunit;

namespace DotNetRetire.Tests
{
    public class FileServiceTests
    {
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
        public void GetDirectoryAndFileWithValidPaths_Success(string path)
        {
            var fileService = new FileService(NullLogger<FileService>.Instance, Options.Create(new RetireServiceOptions { Path = path }));

            Assert.EndsWith(Path.GetFullPath("TestFiles/SingleTarget"), Path.GetFullPath(fileService.GetDirectory()));
            Assert.EndsWith(Path.GetFullPath("TestFiles/SingleTarget/obj/project.assets.json"), Path.GetFullPath(fileService.GetFile()));
        }
    }
}
