using System.Collections.Generic;

namespace RetireNet.Runtimes.Core.HttpClients.Models.Channels
{
    internal class Channel
    {
        public Channel()
        {
            Releases = new List<Release>();
        }
        public IEnumerable<Release> Releases { get; set; }
    }
}

