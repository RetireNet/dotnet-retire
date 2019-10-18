using System.Collections.Generic;

namespace RetireRuntimeMiddleware.HttpClients.Models
{
    internal class ReleasesContainer
    {
        public ReleasesContainer()
        {
            Releases = new List<Release>();
        }
        public IEnumerable<Release> Releases { get; set; }
    }
}
