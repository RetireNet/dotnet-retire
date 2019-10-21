using System.Collections.Generic;
using DotNet.Retire.Packages.Tool.Models;

namespace DotNet.Retire.Packages.Tool.Services
{
    public interface IAssetsFileParser
    {
        IEnumerable<NugetReference> GetNugetReferences();
    }
}
