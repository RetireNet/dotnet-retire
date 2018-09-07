
using NuGet.ProjectModel;

namespace dotnet_retire
{
    public interface IFileService
    {
        LockFile ReadLockFile();
    }
}
