
using NuGet.ProjectModel;

namespace DotNet.Retire.Packages.Tool.Services
{
    public interface IFileService
    {
        LockFile ReadLockFile();
        string GetCurrentDirectory();
    }
}
