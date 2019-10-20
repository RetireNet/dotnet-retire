using System.Collections.Generic;

namespace RetireRuntimeMiddleware.Clients.Models
{
    internal class Channel
    {
        public Channel()
        {
            Releases = new List<Release>();
        }

        public List<Release> Releases { get; set; }

    }
}
