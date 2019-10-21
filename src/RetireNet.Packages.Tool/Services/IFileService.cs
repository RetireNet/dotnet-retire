
using NuGet.ProjectModel;

namespace RetireNet.Packages.Tool.Services
{
    public interface IFileService
    {
        LockFile ReadLockFile();
        string GetCurrentDirectory();
    }
}
