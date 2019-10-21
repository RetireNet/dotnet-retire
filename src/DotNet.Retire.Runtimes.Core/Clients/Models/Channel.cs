using System.Collections.Generic;

namespace DotNet.Retire.Runtimes.Core.Clients.Models
{
    public class Channel
    {
        public Channel()
        {
            Releases = new List<Release>();
        }

        public List<Release> Releases { get; set; }

    }
}
