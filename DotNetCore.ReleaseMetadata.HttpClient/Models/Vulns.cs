using System.Collections.Generic;

namespace DotNetCore.ReleaseMetadata.HttpClient.Models
{
    public class Vulns
    {
        public Vulns()
        {
            Vulnerables = new List<Release>();
        }

        public List<Release> Vulnerables { get; set; }

    }
}