using System.Collections.Generic;
using System.Linq;
using RetireNet.Packages.Tool.Models;

namespace RetireNet.Packages.Tool.Services
{
    public class NugetProjectModelAssetsFileParser : IAssetsFileParser
    {
        private readonly IFileService _fileService;

        public NugetProjectModelAssetsFileParser(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IEnumerable<NugetReference> GetNugetReferences()
        {
            foreach (var lockfile in _fileService.ReadLockFiles())
            {
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
                                    Version = d.VersionRange.MinVersion.OriginalVersion
                                }).ToList()
                        };
                    }
                }
            }
        }
    }
}
