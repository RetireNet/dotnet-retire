
using System.Collections.Generic;
using NuGet.ProjectModel;

namespace RetireNet.Packages.Tool.Services
{
    public interface IFileService
    {
        IEnumerable<LockFile> ReadLockFiles();

        string GetCurrentDirectory();
    }
}
