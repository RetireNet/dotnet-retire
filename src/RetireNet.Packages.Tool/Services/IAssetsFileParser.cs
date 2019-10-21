using System.Collections.Generic;
using RetireNet.Packages.Tool.Models;

namespace RetireNet.Packages.Tool.Services
{
    public interface IAssetsFileParser
    {
        IEnumerable<NugetReference> GetNugetReferences();
    }
}
