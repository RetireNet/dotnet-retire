using System.Collections.Generic;
using System.Linq;
using DotNet.Retire.Packages.Tool.Models;

namespace DotNet.Retire.Packages.Tool.Services
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
            var lockfile = _fileService.ReadLockFile();

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
