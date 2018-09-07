using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using NuGet.ProjectModel;

namespace dotnet_retire
{
    public class NugetProjectModelAssetsFileParser : IAssetsFileParser
    {
        private static NuGetLogger _nugetLogger;

        public NugetProjectModelAssetsFileParser(ILoggerFactory loggerFactory)
        {
            _nugetLogger = new NuGetLogger(loggerFactory.CreateLogger<NuGetLogger>());
        }

        public IEnumerable<NugetReference> GetNugetReferences()
        {
            var lockfile = ReadLockFile();

            foreach (var x in lockfile.Targets)
            {
                foreach (var lib in x.Libraries)
                {
                    yield return new NugetReference
                    {
                        Id = lib.Name,
                        Version = lib.Version.OriginalVersion,
                        Dependencies = lib.Dependencies.Select(d =>
                            new NugetReference
                            {
                                Id = d.Id,
                                Version = d.VersionRange.ToNormalizedString()
                            }).ToList()
                        
                    };
                }
            }
        }

        private static LockFile ReadLockFile()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var objDirectory = Path.Combine(currentDirectory, "obj");
            var lockFilePath = Directory.EnumerateFiles(objDirectory, "project.assets.json", SearchOption.TopDirectoryOnly).FirstOrDefault();
            var lockfile = LockFileUtilities.GetLockFile(lockFilePath, _nugetLogger);
            return lockfile;
        }
    }
}
