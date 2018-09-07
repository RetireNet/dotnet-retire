using System.Collections.Generic;

namespace dotnet_retire
{
    public interface IAssetsFileParser
    {
        IEnumerable<NugetReference> GetNugetReferences();
    }
}
