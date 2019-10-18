using System.Collections.Generic;

namespace DotNetCore.ReleaseMetadata.HttpClient.Models
{
    public class ReleasesContainer
    {
        public ReleasesContainer()
        {
            Releases = new List<Release>();
        }
        public IEnumerable<Release> Releases { get; set; }
    }
}