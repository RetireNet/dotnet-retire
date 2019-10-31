using System.Collections.Generic;
using System.Linq;
using NuGet.ProjectModel;
using RetireNet.Packages.Tool.Models;

namespace RetireNet.Packages.Tool.Services
{
    public class NugetProjectModelAssetsFileParser : IAssetsFileParser
    {
        public IEnumerable<NugetReference> GetNugetReferences(LockFile lockFile)
        {
            foreach (var target in lockFile.Targets)
            {
                foreach (var lib in target.Libraries)
                {
                    yield return new NugetReference
                    {
                        Id = lib.Name,
                        Version = lib.Version.OriginalVersion,
                        Dependencies = lib.Dependencies.Select(d =>
                            new NugetReference
                            {
                                Id = d.Id,
                                Version = d.VersionRange.MinVersion.OriginalVersion
                            }).ToList()
                    };
                }
            }
        }
    }
}
